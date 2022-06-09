using System.Text;
using System.Xml;
using VerizonReports.Models;

namespace VerizonReports.Repository;

public class VerizonApiAdapter
{
    private static readonly List<string>? Mdn = new List<string>();
    private static readonly List<string>? LastActivationDate = new List<string>();
    private static readonly List<string>? Meid = new List<string>();
    private static readonly List<string>? AccountName = new List<string>();
    protected static List<VerizonData> VerizonData = new List<VerizonData>();

    public async Task<List<VerizonData>> PrintDictData()
    {
        await GenerateApiCall();
        CombineData();
        return VerizonData;
    }
    private static async Task<string> PostSoapRequestAsync(string url, string text)
    {
        using var content = new StringContent(text, Encoding.UTF8, "text/xml");
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("SOAPAction", "http://nphase.com/unifiedwebservice/v2/IDeviceService/GetDeviceList");
        request.Content = content;
        var client = new HttpClient();
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
        return await response.Content.ReadAsStringAsync();
    }

    private static async Task<string> LoginPostSoapRequestAsync(string url, string text)
    {
        using var content = new StringContent(text, Encoding.UTF8, "text/xml");
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Add("SOAPAction", "http://nphase.com/unifiedwebservice/v2/ISessionService/LogIn");
        request.Content = content;
        var client = new HttpClient();
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
        return await response.Content.ReadAsStringAsync();
    }
    private static string DeviceRequestTask(string token, string? largestDeviceId)
    {
        var request = @$"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                     xmlns:v2=""http://nphase.com/unifiedwebservice/v2""
                     xmlns:nph=""http://schemas.datacontract.org/2004/07/NPhase.UnifiedWebService.APIs.v2.Contract.DeviceService""
                     xmlns:nph1=""http://schemas.datacontract.org/2004/07/NPhase.UnifiedWebService.APIs.v2.Contract.Common"">
                 <soapenv:Header>
                     <v2:token>{token}</v2:token>
                 </soapenv:Header>
                     <soapenv:Body>
                         <v2:GetDeviceList>
                             <v2:Input>
                                 <nph:AccountName>0342059648-00001</nph:AccountName>
                                 <nph:DeviceStateFilter>Active</nph:DeviceStateFilter>
                                 <nph:LargestDeviceIdSeen>{largestDeviceId}</nph:LargestDeviceIdSeen>
                             </v2:Input>
                         </v2:GetDeviceList>
                     </soapenv:Body>
                 </soapenv:Envelope>";
        return request;
    }

    private static async Task<string> LoginRequestTask()
    {
        const string login = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                                xmlns:v2 =""http://nphase.com/unifiedwebservice/v2""
                                xmlns:nph =""http://schemas.datacontract.org/2004/07/NPhase.UnifiedWebService.APIs.v2.Contract.SessionService"">
                                <soapenv:Header/>
                                    <soapenv:Body>
                                        <v2:LogIn>
                                            <v2:Input>
                                                <nph:Username>PUMPALARMCPNUWS</nph:Username>
                                                <nph:Password>W5ydG$8d</nph:Password>
                                            </v2:Input>
                                        </v2:LogIn>
                                    </soapenv:Body>
                                </soapenv:Envelope>";
        var resultLogin =
            await LoginPostSoapRequestAsync("https://uws.apps.nphase.com/api/v2/SessionService.svc?wsdl", login);
        return resultLogin;
    }

    private static async Task GenerateApiCall()
    {
        try
        {
            var xmlDocument = new XmlDocument();
            var loginXmlDoc = new XmlDocument();
            var largestDeviceId = "0";
            var resultLogin = await LoginRequestTask();
            //The string result from the login request will create an XML doc where the session token can be parsed out  
            loginXmlDoc.LoadXml(resultLogin);
            var sessionToken = loginXmlDoc.GetElementsByTagName("a:SessionToken");
            var token = sessionToken[0]?.InnerText;
            Console.WriteLine(token);
            var flag = "false";
            while (flag == "false")
            {
                //The token and largest Device ID will be used to edit the device request that is sent to the PostSoapRequestAsync 
                var request = DeviceRequestTask(token!, largestDeviceId);
                var isComplete = xmlDocument.GetElementsByTagName("a:IsComplete");
                var isCallComplete = isComplete[0]?.InnerText;
                var result = await PostSoapRequestAsync("https://uws.apps.nphase.com/api/v2/DeviceService.svc?wsdl",
                    request);
                //The string result from this responce will be loaded into an XML doc and that doc will be passed into the argument of 4 different methods 
                xmlDocument.LoadXml(result);

                //These 4 methods will populate the different data field lists: mdn, meid, actDate and accName and the ReturnMdn method will return the largest
                //deviceID to use for the next call to the API
                ReturnLastActivationDate(xmlDocument);
                ReturnAccountNameTask(xmlDocument);
                ReturnMeid(xmlDocument);
                largestDeviceId = ReturnMdn(xmlDocument);

                if (isCallComplete == "true")
                {
                    flag = "true";
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void CombineData()
    {
        for (int i = 0; i < Mdn!.Count; i++)
        {
            var mdn = Mdn[i];
            if (LastActivationDate != null)
            {
                var lastActivationDate = LastActivationDate[i];
                var meid = Meid![i];
                if (AccountName != null)
                {
                    var accountName = AccountName[i];

                    VerizonData.Add(new VerizonData
                    {
                        Mdn = mdn,
                        Meid = meid,
                        LastDateActivated = lastActivationDate,
                        AccountName = accountName
                    });
                }
            }
        }
    }

    private static string? ReturnMdn(XmlDocument xmlDoc)
    {
        var deviceInformationNodes = xmlDoc.GetElementsByTagName("a:DeviceInformation");
        var deviceId = new List<string?>();
        for (var i = 0; i < deviceInformationNodes.Count; i++)
        {
            var deviceIdentifierNode =
                deviceInformationNodes.Item(i)?.SelectSingleNode("*[name()='a:DeviceIdentifiers']");
            if (deviceIdentifierNode == null)
            {
                continue;
            }
            //Start of grabbing extended attributes, which returns one node, and the children are all the ExtendedAttributesOrg.
            var extendedAttributes = deviceInformationNodes.Item(i)
                ?.SelectSingleNode("*[name()='a:ExtendedAttributes']")?.ChildNodes;
            //Get the last child's index since it is always device id.
            var lastAttribute = extendedAttributes?.Count - 1;
            //Get Key
            var kindChild = extendedAttributes?.Item(lastAttribute.GetValueOrDefault())?.FirstChild;
            //Get Value
            var siblingChild = kindChild?.NextSibling;
            var identifiers = deviceIdentifierNode.ChildNodes;
            var list = new List<XmlNode>(identifiers.Cast<XmlNode>());
            foreach (var xmlNode in list)
            {
                if (xmlNode.FirstChild?.InnerText != "mdn")
                {
                    continue;
                }

                var mdn = xmlNode.FirstChild?.NextSibling?.InnerText;
                var device = siblingChild?.InnerText;
                if (mdn == null || device == null)
                {
                    continue;
                }
                Mdn!.Add(mdn);
                deviceId.Add(device);
            }
        }
        if (Mdn != null)
        {
            Console.WriteLine("Mdn Count: " + Mdn.Count);
        }
        return deviceId.LastOrDefault();
    }

    private static void ReturnLastActivationDate(XmlDocument xmlDoc)
    {
        var deviceInformationNodes = xmlDoc.GetElementsByTagName("a:DeviceInformation");
        var lastActivationDateNodes = xmlDoc.GetElementsByTagName("a:LastActivationDate");

        for (var i = 0; i < deviceInformationNodes.Count; i++)
        {
            var deviceIdentifierNode =
                deviceInformationNodes.Item(i)?.SelectSingleNode("*[name()='a:DeviceIdentifiers']");

            if (deviceIdentifierNode == null)
            {
                continue;
            }
            //Get Key
            var activationDateChild = lastActivationDateNodes?.Item(i)?.FirstChild?.InnerText;
            //Get Value
            var identifiers = deviceIdentifierNode.ChildNodes;
            var list = new List<XmlNode>(identifiers.Cast<XmlNode>());
            foreach (var xmlNode in list)
            {
                if (xmlNode.FirstChild?.InnerText != "mdn")
                {
                    continue;
                }

                if (activationDateChild != null)
                {
                    LastActivationDate!.Add(activationDateChild);
                }
            }
        }
        Console.WriteLine("Last Activation Date: " + LastActivationDate!.Count);
    }
    private static void ReturnAccountNameTask(XmlDocument xmlDoc)
    {
        var deviceInformationNodes = xmlDoc.GetElementsByTagName("a:DeviceInformation");
        var accountNameNodes = xmlDoc.GetElementsByTagName("a:AccountName");

        for (var i = 0; i < deviceInformationNodes.Count; i++)
        {
            var deviceIdentifierNode =
                deviceInformationNodes.Item(i)?.SelectSingleNode("*[name()='a:DeviceIdentifiers']");

            if (deviceIdentifierNode == null)
            {
                continue;
            }
            //Get Key
            var accountChild = accountNameNodes?.Item(i)?.FirstChild?.InnerText;

            //Get Value
            var identifiers = deviceIdentifierNode.ChildNodes;
            var list = new List<XmlNode>(identifiers.Cast<XmlNode>());

            foreach (var xmlNode in list)
            {
                if (xmlNode.FirstChild?.InnerText != "mdn")
                {
                    continue;
                }

                if (accountChild != null)
                {
                    AccountName!.Add(accountChild);
                }
            }
        }
        Console.WriteLine("Account Name Count: " + AccountName!.Count);
    }
    private static void ReturnMeid(XmlDocument xmlDoc)
    {
        var deviceInformationNodes = xmlDoc.GetElementsByTagName("a:DeviceInformation");

        for (var i = 0; i < deviceInformationNodes.Count; i++)
        {
            var deviceIdentifierNode =
                deviceInformationNodes.Item(i)?.SelectSingleNode("*[name()='a:DeviceIdentifiers']");
            if (deviceIdentifierNode == null)
            {
                continue;
            }
            //Get Key
            //Get Value
            var identifiers = deviceIdentifierNode.ChildNodes;
            var list = new List<XmlNode>(identifiers.Cast<XmlNode>());
            foreach (var xmlNode in list)
            {
                if (xmlNode.FirstChild?.InnerText != "meid" & xmlNode.FirstChild?.InnerText != "iccId")
                {
                    continue;
                }
                var meid = xmlNode.FirstChild?.NextSibling?.InnerText;

                if (meid == null)
                {
                    continue;
                }
                if (Meid != null)
                {
                    Meid.Add(meid);
                }
            }
        }
        Console.WriteLine("Meid Count " + Meid!.Count);
    }
}