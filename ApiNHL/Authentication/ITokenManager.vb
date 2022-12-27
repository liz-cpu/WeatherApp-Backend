Public Interface ITokenManager
    Function Authenticate(Username As String) As Boolean

    Function NewToken(Usr As Model.User, Optional Claims As Dictionary(Of String, Object) = Nothing) As Token

    Function GetToken(Value As String) As Token

    Function VerifyToken(Value As String) As Boolean
End Interface
