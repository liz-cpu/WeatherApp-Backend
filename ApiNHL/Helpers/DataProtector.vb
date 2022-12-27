Imports System.Security.Cryptography

Public Class DataProtector
    Private Shared ReadOnly Entropy As Byte() = {145, 131, 15, 215, 110, 198, 117, 21, 207, 110, 212, 186, 115, 207, 92, 228}

    ''' <summary>
    ''' Encrypt de meegegeven tekst. De encryptie is gebaseerd op de ingelogde gebruiker van de machine.
    ''' </summary>
    ''' <param name="PlainText">Tekst</param>
    ''' <returns>Een ge-encrypte string</returns>
    Public Shared Function Protect(PlainText As String) As String
        Dim EncryptedBytes As Byte() = ProtectedData.Protect(Encoding.UTF8.GetBytes(PlainText),
                                                             Entropy, DataProtectionScope.LocalMachine)
        Return Convert.ToBase64String(EncryptedBytes)
    End Function

    ''' <summary>
    ''' Decrypt de meegeven encrypte tekst. Deze tekst moet op dezelfde machine met dezelfde gebruiker
    ''' zijn versleuteld.
    ''' </summary>
    ''' <param name="EncryptedText">Een ge-encrypte tekst</param>
    ''' <returns>De originele waarde van de meegegen waarde.</returns>
    Public Shared Function Unprotect(EncryptedText As String) As String
        Dim DecryptedBytes As Byte() = ProtectedData.Unprotect(Convert.FromBase64String(EncryptedText),
                                                     Entropy, DataProtectionScope.LocalMachine)
        Return Encoding.UTF8.GetString(DecryptedBytes)
    End Function
End Class
