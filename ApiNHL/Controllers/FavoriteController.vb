Imports System.Web.Http
Imports ApiNHL.ApiNHL.Filters
Imports ApiNHL.Model
Imports ApiNHL.NHL.Requests
Imports Newtonsoft.Json

Namespace Controllers
    <TokenAuthenticationFilter>
    Public Class FavoriteController
        Inherits ApiController

        <HttpPost>
        <ActionName("add")>
        Public Function AddFavorite(Request As FavoriteRequest) As IHttpActionResult
            If IsNothing(Request) OrElse
                    IsNothing(Request.Place) Then
                Return BadRequest("Plaats onbekend.")
            End If

            Dim Favorites As List(Of Favorite) = Favorite.GetAllFromLoggedInUser

            If Not IsNothing(Favorites.Where(Function(F) F.Name.ToLower.Equals(Request.Place.ToLower)).FirstOrDefault) Then
                Return BadRequest("Deze plaats staat al in je favorieten.")
            ElseIf Favorites.Count.Equals(3) Then
                Return BadRequest("Je kan maar 3 favorite plaatsen hebben.")
            End If

            Dim Fav As New Favorite(Request.Place, ClaimManager.GetClaimValue("id"))
            Fav.Save()

            Return Ok("Plaats is toegevoegd")
        End Function

        <HttpPost>
        <ActionName("delete")>
        Public Function DeleteFavorite(Request As FavoriteRequest) As IHttpActionResult
            If IsNothing(Request) OrElse
                IsNothing(Request.Place) Then
                Return BadRequest("Plaats onbekend.")
            End If

            Dim Fav As New Favorite(Request.Place, ClaimManager.GetClaimValue("id"))
            Fav.Delete()

            Return Ok("Plaats is verwijderd")
        End Function

        <HttpPost>
        <ActionName("all")>
        Public Function GetAll() As IHttpActionResult
            Return Ok(JsonConvert.SerializeObject(Favorite.GetAllFromLoggedInUser))
        End Function
    End Class
End Namespace