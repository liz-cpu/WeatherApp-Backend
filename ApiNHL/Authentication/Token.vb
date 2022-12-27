Imports ApiNHL.Model

Public Class Token
    Private _Value As String
    Private _ExpiryDate As DateTime
    Private _User As User
    Private _Claims As Dictionary(Of String, Object)

    Public Sub New(Token As String, Expires As DateTime, Usr As User, Claims As Dictionary(Of String, Object))
        Me.Value = Token
        Me.ExpiryDate = Expires
        Me.User = Usr
        Me.Claims = Claims
    End Sub

    Public Property Value As String
        Get
            Return String.Format("NHL {0}", _Value)
        End Get
        Private Set(value As String)
            _Value = value
        End Set
    End Property

    Public Property ExpiryDate As Date
        Get
            Return _ExpiryDate
        End Get
        Private Set(value As Date)
            _ExpiryDate = value
        End Set
    End Property

    Public Property User As User
        Get
            Return _User
        End Get
        Set(value As User)
            _User = value
        End Set
    End Property

    Public Property Claims As Dictionary(Of String, Object)
        Get
            Return _Claims
        End Get
        Private Set(value As Dictionary(Of String, Object))
            _Claims = value
        End Set
    End Property
End Class
