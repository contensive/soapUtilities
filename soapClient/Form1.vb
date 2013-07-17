Imports System.Xml
Imports Microsoft.Web.Services3
Imports Microsoft.Web.Services3.Addressing
Imports Microsoft.Web.Services3.Messaging
Imports System.Text
Imports CSC = ccCSrvr3
Imports soapClass = aoSOAP.utilClass

Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim sysURI As New System.Uri(requestAddr.Text)
        Dim eop As New Microsoft.Web.Services3.Addressing.EndpointReference(sysURI)
        Dim se As New Microsoft.Web.Services3.SoapEnvelope()
        Dim ReturnEnvelope As New Microsoft.Web.Services3.SoapEnvelope()
        Dim xmlNode As XmlNode
        Dim objTCP As New aoSOAP.TcpClient(eop)
        Dim opObj As New XmlDocument
        Dim exFromSOAP As String = ""
        Dim soap As New soapClass()
        Dim decodedBytes As Byte()
        Dim decodedText As String = ""
        Dim errorNumber As Integer = 0
        Dim errorMessage As String = ""

        Dim ReturnValue As New CSC.CSConnectionType
        Dim CSV As New CSC.ContentServerClass

        ReturnValue = CSV.OpenConnection("TCA")

        'If ReturnValue.ApplicationStatus = 2 Then
        se.InnerXml = reqEnv.Text
        soap.getEnvelopeContents(requestAddr.Text, reqEnv.Text, "ns1:SoapServicesBlobResponse", errorNumber, errorMessage)
        ReturnEnvelope = objTCP.RequestResponseMethod(se, errorNumber, errorMessage)

        If Not ReturnEnvelope Is Nothing Then
            For Each xmlNode In ReturnEnvelope.Body.ChildNodes
                If xmlNode.Name = "ns1:SoapServicesBlobResponse" Then
                    decodedBytes = Convert.FromBase64String(ReturnEnvelope.Body.InnerText)
                    decodedText = Encoding.UTF8.GetString(decodedBytes)
                    TextBox4.Text = decodedText
                    opObj.LoadXml(decodedText)
                End If
            Next
            TextBox3.Text = ReturnEnvelope.Body.InnerXml
        Else
            TextBox3.Text = exFromSOAP
        End If
        'Else
        'TextBox3.Text = "App Not Running"
        'TextBox4.Text = "App Not Running"
        'End If

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        reqEnv.Text = "<?xml version=""1.0"" encoding=""ISO-8859-1""?>" _
                    & "<SOAP-ENV:Envelope SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:tns=""http://www.MemberMax.com/namespace/default"">" _
                    & "<SOAP-ENV:Body>" _
                    & "<tns:SoapServicesBlob xmlns:tns=""http://www.MemberMax.com/namespace/default"">" _
                    & "<inText xsi:type=""xsd:string"">" _
& "&lt;?xml version=&quot;1.0&quot; encoding=&quot;ISO-8859-1&quot;?&gt;" _
& "&lt;RequestedMethodWrapper&gt;" _
& "&lt;RequestedMethodParams&gt;" _
& "&lt;MbrxSoapLicenseNo&gt;MBR9056592&lt;/MbrxSoapLicenseNo&gt;" _
& "&lt;DataReturnFormat&gt;XML&lt;/DataReturnFormat&gt;" _
& "&lt;ServerDebugLog&gt;Off&lt;/ServerDebugLog&gt;  " _
& "&lt;RequestedMethod&gt;CompanyDetail&lt;/RequestedMethod&gt;" _
& "&lt;Companyfilter&gt;2302&lt;/Companyfilter&gt;" _
& "&lt;KeyWordFilter&gt;All&lt;/KeyWordFilter&gt;" _
& "&lt;RelatedPersonfilter&gt;All&lt;/RelatedPersonfilter&gt;" _
& "&lt;ChaptersFilter&gt;All&lt;/ChaptersFilter&gt;" _
& "&lt;RelatedRecordsFilter&gt;All&lt;/RelatedRecordsFilter&gt;" _
& "&lt;/RequestedMethodParams&gt;" _
& "&lt;/RequestedMethodWrapper&gt;" _
                    & "</inText>" _
                    & "</tns:SoapServicesBlob>" _
                    & "</SOAP-ENV:Body>" _
                    & "</SOAP-ENV:Envelope>"
    End Sub

End Class