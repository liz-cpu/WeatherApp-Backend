Namespace NHL.Requests
    Public Class RegisterRequest
        Private _Username As String
        Private _Password1 As String
        Private _Password2 As String

        Public Property Username As String
            Get
                Return _Username
            End Get
            Set(value As String)
                _Username = value
            End Set
        End Property

        Public Property Password1 As String
            Get
                Return _Password1
            End Get
            Set(value As String)
                _Password1 = value
            End Set
        End Property

        Public Property Password2 As String
            Get
                Return _Password2
            End Get
            Set(value As String)
                _Password2 = value
            End Set
        End Property
    End Class
End Namespace