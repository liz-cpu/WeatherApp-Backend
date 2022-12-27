Namespace NHL.Requests
    Public Class FavoriteRequest
        Private _Place As String

        Public Property Place As String
            Get
                Return _Place
            End Get
            Set(value As String)
                _Place = value
            End Set
        End Property
    End Class
End Namespace