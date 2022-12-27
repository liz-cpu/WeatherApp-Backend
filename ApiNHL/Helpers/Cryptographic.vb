Imports System.Security.Cryptography

Public Class Cryptographic
    ''' <summary>
    ''' Hashed de meegegeven tekst met SHA512, een random salt en een pepper
    ''' </summary>
    ''' <param name="Input">Tekst die je wilt hashen.</param>
    ''' <returns>Een object waarin de gehashte waarde staat samen met de salt.</returns>
    Public Shared Function HashSHA512(Input As String) As SHA512Hash
        Dim CombinedBytes As New List(Of Byte)

        Dim SaltBytes As Byte() = GenerateSalt()

        CombinedBytes.AddRange(Encoding.UTF8.GetBytes(Input))
        CombinedBytes.AddRange(SaltBytes)
        CombinedBytes.AddRange(Encoding.UTF8.GetBytes(DataProtector.Unprotect(ConfigurationManager.AppSettings.Item("Pepper"))))

        Dim HashedBytes As Byte()
        Using Hasher As SHA512 = SHA512.Create()
            HashedBytes = Hasher.ComputeHash(CombinedBytes.ToArray)
        End Using

        Return New SHA512Hash(Convert.ToBase64String(HashedBytes), Convert.ToBase64String(SaltBytes))
    End Function

    ''' <summary>
    ''' Hashed de meegegeven tekst met SHA512 en een random salt. Voegt geen pepper toe.
    ''' </summary>
    ''' <param name="Input"></param>
    ''' <param name="SaltLength"></param>
    ''' <returns></returns>
    Public Shared Function HashSHA512(Input As String, SaltLength As Integer) As String
        Dim CombinedBytes As New List(Of Byte)

        CombinedBytes.AddRange(Encoding.UTF8.GetBytes(Input))
        CombinedBytes.AddRange(GenerateSalt(SaltLength))

        Dim HashedBytes As Byte()
        Using Hasher As SHA512 = SHA512.Create()
            HashedBytes = Hasher.ComputeHash(CombinedBytes.ToArray)
        End Using

        Return Convert.ToBase64String(HashedBytes)
    End Function

    ''' <summary>
    ''' Hashed de meegegeven tekst met SHA512, de meegegeven salt en een pepper
    ''' </summary>
    ''' <param name="Input">Tekst die je wilt hashen.</param>
    ''' <param name="Salt">De tekst die je wilt bijvoegen voor het hashen.</param>
    ''' <returns>De gehashte waarde.</returns>
    Private Shared Function HashSHA512(Input As String, Salt As String) As String
        Dim CombinedBytes As New List(Of Byte)

        CombinedBytes.AddRange(Encoding.UTF8.GetBytes(Input))
        CombinedBytes.AddRange(Convert.FromBase64String(Salt))
        CombinedBytes.AddRange(Encoding.UTF8.GetBytes(DataProtector.Unprotect(ConfigurationManager.AppSettings.Item("Pepper"))))

        Dim HashedBytes As Byte()
        Using Hasher As SHA512 = SHA512.Create()
            HashedBytes = Hasher.ComputeHash(CombinedBytes.ToArray)
        End Using

        Return Convert.ToBase64String(HashedBytes)
    End Function

    ''' <summary>
    ''' Controleert of stuk tekst gelijk is aan de gehashte waarde.
    ''' </summary>
    ''' <param name="Password">Plain text wachtwoord</param>
    ''' <param name="Salt"></param>
    ''' <param name="HashedPassword">Een wachtwoord dat ook met SHA512 is gehasht.</param>
    ''' <returns></returns>
    Private Shared Function ValidateSHA512(Password As String, Salt As String, HashedPassword As String) As Boolean
        Try
            Return SafeEquals(HashSHA512(Password, Salt), HashedPassword)
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Krijgt een wachtwoord mee en een opgeslagen wachtwoord. Zoekt uit waarmee het opgeslagen
    ''' wachtwoord is beveiligd en gaat dan kijken of er een overeenkomst is.
    ''' </summary>
    ''' <param name="Password">Plain tekst wachtwoord</param>
    ''' <param name="StoredPassword">Gehashte wachtwoord</param>
    ''' <returns>Als de waardes overeen komen true, anders false.</returns>
    Public Shared Function ValidatePassword(Password As String, StoredPassword As String) As Boolean
        Try
            StoredPassword = RSAEncryption.Decrypt(StoredPassword)
        Catch ex As Exception
            Return False
        End Try

        Dim Parts As String() = StoredPassword.Split(":")

        If Parts.Length.Equals(1) Then
            Return False
        End If

        Dim Algorithm As String = Parts.ElementAt(0)

        If Algorithm.Equals("SHA512") Then
            If Not Parts.Length.Equals(3) Then Return False

            Return ValidateSHA512(Password, Parts.ElementAt(2), Parts.ElementAt(1))
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Controleert of 2 waardes overeen komen. Maar gaat ook door met kijken als er al zeker
    ''' is dat ze niet overeen komen. Dit is om een timing attack te voorkomen.
    ''' </summary>
    ''' <param name="ExternalInput">Invoer van buiten de applicatie.</param>
    ''' <param name="InternalInput">Invoer van binnen de applicatie.</param>
    ''' <returns>Als de waardes overeen komen true, anders false.</returns>
    Public Shared Function SafeEquals(ExternalInput As String, InternalInput As String) As Boolean
        Dim ExternalChars As Char() = ExternalInput.ToCharArray
        Dim InternalChars As Char() = InternalInput.ToCharArray

        Dim Match As Boolean = True

        For I As Integer = 0 To ExternalChars.Length - 1
            Dim C1 As Char = ExternalChars.ElementAtOrDefault(I)
            Dim C2 As Char = InternalChars.ElementAtOrDefault(I)
            Match = (Match And C1 = C2 And C2 <> vbNullChar)
        Next

        Return (Match And ExternalInput.Length = InternalInput.Length)
    End Function

    ''' <summary>
    ''' Genereert een random salt van 64 byte
    ''' </summary>
    ''' <returns>Een random salt</returns>
    Public Shared Function GenerateSalt(Optional Length As Integer = 64) As Byte()
        Dim Bytes(Length - 1) As Byte

        Using RNGProvider As New RNGCryptoServiceProvider
            RNGProvider.GetNonZeroBytes(Bytes)
        End Using
        Return Bytes
    End Function
End Class

Public Class SHA512Hash
    Private _Digest As String
    Private _Salt As String

    Public Sub New(Digest As String, Salt As String)
        Me.Digest = Digest
        Me.Salt = Salt
    End Sub

    Public Property Digest As String
        Get
            Return _Digest
        End Get
        Private Set(value As String)
            _Digest = value
        End Set
    End Property

    Public Property Salt As String
        Get
            Return _Salt
        End Get
        Private Set(value As String)
            _Salt = value
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return RSAEncryption.Encrypt(String.Format("SHA512:{0}:{1}", Me.Digest, Me.Salt))
    End Function
End Class
