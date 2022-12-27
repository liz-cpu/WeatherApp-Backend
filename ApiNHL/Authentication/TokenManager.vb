Imports ApiNHL.Model
Imports Google.Protobuf.WellKnownTypes

Public Class TokenManager
    Implements ITokenManager

    Private ReadOnly ActiveTokens As List(Of Token) = New List(Of Token)

    Public Sub New()
        Me.ActiveTokens = New List(Of Token)
    End Sub

    Public Function Authenticate(Username As String) As Boolean Implements ITokenManager.Authenticate
        Return Username.ToLower.Equals("admin")
    End Function

    Public Function NewToken(Usr As User, Optional Claims As Dictionary(Of String, Object) = Nothing) As Token Implements ITokenManager.NewToken
        Dim TokenValue As String = Cryptographic.HashSHA512(Guid.NewGuid().ToString, 32)

        While TokenValueExists(TokenValue)
            TokenValue = Cryptographic.HashSHA512(Guid.NewGuid().ToString, 32)
        End While


        Dim Tkn As New Token(TokenValue, DateTime.Now.AddDays(1), Usr, If(Claims, New Dictionary(Of String, Object)))

        AddToken(Tkn)

        Return Tkn
    End Function

    Public Function VerifyToken(Value As String) As Boolean Implements ITokenManager.VerifyToken
        Return Not IsNothing(Me.ActiveTokens.Where(Function(T) T.Value.Equals(Value) AndAlso T.ExpiryDate > Now).FirstOrDefault)
    End Function

    Public Function GetToken(Value As String) As Token Implements ITokenManager.GetToken
        Return Me.ActiveTokens.Where(Function(T) T.Value.Equals(Value) AndAlso T.ExpiryDate > Now).FirstOrDefault
    End Function

    Private Function TokenValueExists(Value As String) As Boolean
        Return Not IsNothing(Me.ActiveTokens.Where(Function(T) T.Value.Equals(Value)).FirstOrDefault)
    End Function

    Private Sub AddToken(Tkn As Token)
        Dim OldTkn As Token = ActiveTokens.Where(Function(T) T.User.Id.Equals(Tkn.User.Id)).FirstOrDefault

        If Not IsNothing(OldTkn) Then
            ActiveTokens.Remove(OldTkn)
        End If

        ActiveTokens.Add(Tkn)
    End Sub
End Class
