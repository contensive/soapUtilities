Imports System.Xml
Imports Microsoft.Web.Services3
Imports Microsoft.Web.Services3.Addressing
Imports Microsoft.Web.Services3.Messaging
Imports System.Text

Public Class Form1
    '
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        '
        Dim sysURI As New System.Uri(requestAddr.Text)
        Dim eop As New Microsoft.Web.Services3.Addressing.EndpointReference(sysURI)
        Dim se As New Microsoft.Web.Services3.SoapEnvelope()
        Dim ReturnEnvelope As New Microsoft.Web.Services3.SoapEnvelope()
        Dim xmlNode As XmlNode
        'Dim objTCP As New Contensive.soap.TcpClient(eop)
        Dim opObj As New XmlDocument
        Dim exFromSOAP As String = ""
        Dim soap As New Contensive.soap.utilClass
        Dim decodedBytes As Byte()
        Dim decodedText As String = ""
        Dim errorNumber As Integer = 0
        Dim errorMessage As String = ""
        '
        reqEnv.Text = "<?xml version=""1.0"" encoding=""ISO-8859-1""?>"
        reqEnv.Text += "<SOAP-ENV:Envelope SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:tns=""http://www.MemberMax.com/namespace/default"">"
        reqEnv.Text += "<SOAP-ENV:Body>"
        reqEnv.Text += "<tns:transferData xmlns:tns=""http://info.asm.org/ws/phpSoap/ASMSoapServer.php"">"
        reqEnv.Text += "<upc xsi:type=""xsd:string"">"
        reqEnv.Text += contents.Text
        reqEnv.Text += "</upc>"
        reqEnv.Text += "</tns:transferData>"
        reqEnv.Text += "</SOAP-ENV:Body>"
        reqEnv.Text += "</SOAP-ENV:Envelope>"
        '
        se.InnerXml = reqEnv.Text
        soap.getResponse(requestAddr.Text, reqEnv.Text, "ns1:SoapServicesBlobResponse", errorNumber, errorMessage)
        'ReturnEnvelope = objTCP.RequestResponseMethod(se, errorNumber, errorMessage)
        '
        If Not ReturnEnvelope Is Nothing Then
            'For Each xmlNode In ReturnEnvelope.Body.ChildNodes
            '    If xmlNode.Name = "ns1:SoapServicesBlobResponse" Then
            '        decodedBytes = Convert.FromBase64String(ReturnEnvelope.Body.InnerText)
            '        decodedText = Encoding.UTF8.GetString(decodedBytes)
            '        TextBox4.Text = decodedText
            '        opObj.LoadXml(decodedText)
            '    End If
            'Next
            TextBox3.Text = ReturnEnvelope.Body.InnerXml
        Else
            TextBox3.Text = exFromSOAP
        End If
        '
    End Sub
    '
End Class