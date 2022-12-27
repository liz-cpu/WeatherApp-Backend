Imports System.Web
Imports System.Web.Mvc
Imports ApiNHL.ApiNHL.Filters
Imports Org.BouncyCastle.Math.EC.ECCurve

Public Module FilterConfig
    Public Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)
        filters.Add(New HandleErrorAttribute())
        'filters.Add(New TokenAuthenticationFilter())
    End Sub
End Module