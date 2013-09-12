Imports System.Xml
Imports Microsoft.Web.Services3
Imports Microsoft.Web.Services3.Addressing
Imports Microsoft.Web.Services3.Messaging
Imports System.Text
Imports httpUtil = System.Web.HttpUtility
'
Namespace Contensive.soap
    '
    Public Class utilClass
        '
        Public Function getResponse(ByVal serviceURI As String, ByVal requestEnvelope As String, ByVal responseNode As String, ByRef responseErrorNumber As Integer, ByRef responseErrorMessage As String) As String
            Dim soapResponse As String = ""
            '
            Try
                Dim sysURI As New System.Uri(serviceURI)
                Dim eop As New Microsoft.Web.Services3.Addressing.EndpointReference(sysURI)
                Dim se As New Microsoft.Web.Services3.SoapEnvelope()
                Dim objTCP As New TcpClient(eop)
                Dim fileContent As String = ""
                '
                Dim returnEnvelope As New Microsoft.Web.Services3.SoapEnvelope()
                '
                se.InnerXml = requestEnvelope
                fileContent = se.ToString
                '
                System.IO.File.AppendAllText("c:\temp\seleralizedXML.txt", fileContent)
                '
                returnEnvelope = objTCP.RequestResponseMethod(se, responseErrorNumber, responseErrorMessage)
                '
                If Not returnEnvelope Is Nothing Then
                    For Each node As XmlNode In returnEnvelope.SelectNodes("//Result")
                        soapResponse = node.InnerText
                    Next
                End If
            Catch ex As Exception
                Call HandleError("Contensive.soap.utilClass.getResponse", Err.Number, Err.Source, Err.Description)
            End Try
            '
            Return soapResponse
            '
        End Function
        '
        Public Function getEnvelopex(ByVal method As String, ByVal endPoint As String, ByVal dataNode As String, ByVal envelopeContents As String) As String
            Dim s As String = ""
            '
            Try
                s = "<?xml version=""1.0"" encoding=""ISO-8859-1""?>"
                s += "<SOAP-ENV:Envelope SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:tns=""http://www.MemberMax.com/namespace/default"">"
                s += "<SOAP-ENV:Body>"
                s += "<tns:" & method & " xmlns:tns=""" & endPoint & """>"
                s += "<" & dataNode & " xsi:type=""xsd:string"">"
                s += httpUtil.HtmlEncode(envelopeContents)
                s += "</" & dataNode & ">"
                s += "</tns:" & method & ">"
                s += "</SOAP-ENV:Body>"
                s += "</SOAP-ENV:Envelope>"
            Catch ex As Exception
                Call HandleError("Contensive.soap.utilClass.getEnvelope", Err.Number, Err.Source, Err.Description)
            End Try
            '
            Return s
        End Function
        '
        Private Sub appendLog(ByVal copy As String)
            Try
                My.Computer.FileSystem.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory() & "log.txt", Now & " - " & copy & vbCrLf, True)
            Catch ex As Exception
                Call HandleError("Contensive.soap.utilClass.appendLog", Err.Number, Err.Source, Err.Description)
            End Try
        End Sub
        '
        Private Sub HandleError(ByVal MethodName As String, ByVal ErrNumber As Long, ByVal ErrSource As String, ByVal ErrDescription As String)
            appendLog("Error in " & MethodName & " - " & ErrDescription & " [" & ErrNumber & " - " & ErrSource & "]")
        End Sub
        '
    End Class
    '
    Public Class TcpClient
        Inherits SoapClient
        '
        Public Sub New(ByVal destination As EndpointReference)
            MyBase.New(destination)
        End Sub
        '
        <SoapMethod("RequestResponseMethod")> Public Function RequestResponseMethod(ByVal envelope As SoapEnvelope, ByRef errorNumber As Integer, ByRef errorMessage As String) As SoapEnvelope
            '
            Try
                Return MyBase.SendRequestResponse("RequestResponseMethod", envelope)
            Catch ex As Exception
                errorNumber = Err.Number
                errorMessage = ex.Message.ToString
            End Try
            '
        End Function
        '
        Private Sub appendLog(ByVal copy As String)
            My.Computer.FileSystem.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory() & "log.txt", Now & " - " & copy & vbCrLf, True)
        End Sub
        '
        Private Sub HandleError(ByVal MethodName As String, ByVal ErrNumber As Long, ByVal ErrSource As String, ByVal ErrDescription As String)
            appendLog("Error in " & MethodName & " - " & ErrDescription & " [" & ErrNumber & " - " & ErrSource & "]")
        End Sub
        '
    End Class
    '
End Namespace