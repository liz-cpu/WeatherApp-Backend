Namespace NHL.Requests
    Public Class LoginRequest
        Private _Username As String
        Private _Password As String

        Public Property Username As String
            Get
                Return _Username
            End Get
            Set(value As String)
                _Username = value
            End Set
        End Property

        Public Property Password As String
            Get
                Return _Password
            End Get
            Set(value As String)
                _Password = value
            End Set
        End Property
    End Class
End Namespace