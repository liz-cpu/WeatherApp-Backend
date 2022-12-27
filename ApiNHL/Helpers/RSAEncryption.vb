Imports System.Security.Cryptography
Imports Org.BouncyCastle.Utilities.IO.Pem

Public Class RSAEncryption

    ''' <summary>
    ''' Versleuteld de meegegeven tekst met de public key.
    ''' </summary>
    ''' <param name="PlainText"></param>
    ''' <returns>Versleutelde tekst</returns>
    Public Shared Function Encrypt(PlainText As String) As String
        Dim PlainBytes As Byte() = Encoding.UTF8.GetBytes(PlainText)

        Dim Provider As New RSACryptoServiceProvider

        Provider.FromXmlString(DataProtector.Unprotect(ConfigurationManager.AppSettings.Item("PublicKey")))

        Return Convert.ToBase64String(Provider.Encrypt(PlainBytes, RSAEncryptionPadding.Pkcs1))
    End Function

    ''' <summary>
    ''' Ontsleuteld de meegegeven tekst met de private key.
    ''' </summary>
    ''' <param name="EncryptedText"></param>
    ''' <returns>De originele waarde van meegegeven tekst.</returns>
    Public Shared Function Decrypt(EncryptedText As String, Optional Key As String = Nothing) As String
        Dim EncryptedBytes As Byte() = Convert.FromBase64String(EncryptedText)

        Dim Provider As New RSACryptoServiceProvider

        If IsNothing(Key) Then
            Provider.FromXmlString(DataProtector.Unprotect(ConfigurationManager.AppSettings.Item("PrivateKey")))
        Else
            Provider.FromXmlString(Key)
        End If

        Return Encoding.UTF8.GetString(Provider.Decrypt(EncryptedBytes, RSAEncryptionPadding.Pkcs1))
    End Function
End Class
