using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfPasonServices
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class WitsmlService : IWitsmlService
    {
        public string ProcessRequest(string soapMessage)
        {
            // Guardar mensaje SOAP
            File.AppendAllText("data_storage.txt", soapMessage + "\n");

            // Responder en función del mensaje SOAP
            if (soapMessage.Contains("WMLS_GetVersion"))
            {
                return @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <soapenv:Body>
                                <WMLS_GetVersionResponse>
                                    <Result>1.4.1.1</Result>
                                </WMLS_GetVersionResponse>
                            </soapenv:Body>
                        </soapenv:Envelope>";
            }
            else if (soapMessage.Contains("WMLS_GetCap"))
            {
                return @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <soapenv:Body>
                                <WMLS_GetCapResponse>
                                    <Result>1</Result>
                                    <Capabilities>
                                        <capServer>
                                            <function>WMLS_GetFromStore</function>
                                            <function>WMLS_AddToStore</function>
                                            <function>WMLS_UpdateInStore</function>
                                            <function>WMLS_DeleteFromStore</function>
                                        </capServer>
                                    </Capabilities>
                                </WMLS_GetCapResponse>
                            </soapenv:Body>
                        </soapenv:Envelope>";
            }
            else if (soapMessage.Contains("WMLS_GetFromStore") && soapMessage.Contains("wellbore"))
            {
                return @"<?xml version=""1.0"" encoding=""UTF-8""?>
                        <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                            <soapenv:Body>
                                <ns1:WMLS_GetFromStoreResponse xmlns:ns1=""http://www.witsml.org/message/120"">
                                    <Result>1</Result>
                                    <XMLout>&lt;wells xmlns=&quot;http://www.witsml.org/schemas/1series&quot; version=&quot;1.4.1.1&quot;&gt;
                                        &lt;well uid=&quot;well_test&quot;&gt;
                                            &lt;name&gt;well_test&lt;/name&gt;
                                            <!-- Other well details -->
                                        &lt;/well&gt;
                                    &lt;/wells&gt;
                                    </XMLout>
                                    <SuppMsgOut>Success</SuppMsgOut>
                                </ns1:WMLS_GetFromStoreResponse>
                            </soapenv:Body>
                        </soapenv:Envelope>";
            }
            else
            {
                return "Invalid Request";
            }
        }
    }
}
