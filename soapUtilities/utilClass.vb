Imports System.Xml
Imports Microsoft.Web.Services3
Imports Microsoft.Web.Services3.Addressing
Imports Microsoft.Web.Services3.Messaging
Imports System.Text

Public Class utilClass

    Public Function getEnvelopeContents(ByVal serviceURI As String, ByVal requestEnvelope As String, ByVal responseNode As String, ByRef responseErrorNumber As Integer, ByRef responseErrorMessage As String) As String
        On Error GoTo ErrorTrap

        Dim sysURI As New System.Uri(serviceURI)
        Dim eop As New Microsoft.Web.Services3.Addressing.EndpointReference(sysURI)
        Dim se As New Microsoft.Web.Services3.SoapEnvelope()
        Dim objTCP As New TcpClient(eop)

        Dim ReturnEnvelope As New Microsoft.Web.Services3.SoapEnvelope()
        Dim xmlNode As XmlNode
        Dim soapResponse As String = ""

        Dim decodedBytes As Byte()

        se.InnerXml = requestEnvelope
        ReturnEnvelope = objTCP.RequestResponseMethod(se, responseErrorNumber, responseErrorMessage)

        If Not ReturnEnvelope Is Nothing Then
            For Each xmlNode In ReturnEnvelope.Body.ChildNodes
                If xmlNode.Name = responseNode Then
                    decodedBytes = Convert.FromBase64String(ReturnEnvelope.Body.InnerText)
                    soapResponse = Encoding.UTF8.GetString(decodedBytes)
                End If
            Next
        End If

        Return soapResponse

        Exit Function
ErrorTrap:
        Call HandleError("getEnvelopeContents", Err.Number, Err.Source, Err.Description)
        Err.Clear()
        Resume Next
    End Function

    Private Sub appendLog(ByVal copy As String)
        My.Computer.FileSystem.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory() & "log.txt", Now & " - " & copy & vbCrLf, True)
    End Sub

    Private Sub HandleError(ByVal MethodName As String, ByVal ErrNumber As Long, ByVal ErrSource As String, ByVal ErrDescription As String)
        appendLog("Error in " & MethodName & " - " & ErrDescription & " [" & ErrNumber & " - " & ErrSource & "]")
    End Sub

End Class

Public Class TcpClient
    Inherits SoapClient

    Public Sub New(ByVal destination As EndpointReference)
        MyBase.New(destination)
    End Sub

    <SoapMethod("RequestResponseMethod")> Public Function RequestResponseMethod(ByVal envelope As SoapEnvelope, ByRef errorNumber As Integer, ByRef errorMessage As String) As SoapEnvelope

        Try
            Return MyBase.SendRequestResponse("RequestResponseMethod", envelope)
        Catch ex As Exception
            errorNumber = Err.Number
            errorMessage = ex.Message.ToString
        End Try

    End Function

    Private Sub appendLog(ByVal copy As String)
        My.Computer.FileSystem.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory() & "log.txt", Now & " - " & copy & vbCrLf, True)
    End Sub

    Private Sub HandleError(ByVal MethodName As String, ByVal ErrNumber As Long, ByVal ErrSource As String, ByVal ErrDescription As String)
        appendLog("Error in " & MethodName & " - " & ErrDescription & " [" & ErrNumber & " - " & ErrSource & "]")
    End Sub

End Class