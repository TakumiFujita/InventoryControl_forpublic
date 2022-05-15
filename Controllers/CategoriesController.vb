Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports InventoryControl.Models

Namespace Controllers
    Public Class CategoriesController
        Inherits System.Web.Mvc.Controller

        Private db As New InventoryControlContext

        ' GET: Categories
        Function Index() As ActionResult
            Return View(db.Categories.ToList())
        End Function

        ' GET: Categories/Details/5
        Function Details(ByVal id As String) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim category As Category = db.Categories.Find(id)
            If IsNothing(category) Then
                Return HttpNotFound()
            End If
            Return View(category)
        End Function

        ' GET: Categories/Create
        Function Create() As ActionResult
            Return View()
        End Function

        ' POST: Categories/Create
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="CategoryId,CategoryName")> ByVal category As Category) As ActionResult
            If ModelState.IsValid Then
                db.Categories.Add(category)
                Try
                    db.SaveChanges()
                Catch ex As DbUpdateException When TryCast(ex.InnerException?.InnerException, SqlException) IsNot Nothing
                    Dim sqlEx As SqlException = TryCast(ex.InnerException?.InnerException, SqlException)
                    If sqlEx.Number = 2627 Then
                        ModelState.AddModelError("EM_DuplicateCategoryId", "このカテゴリー番号は既に登録されています")
                    End If
                    If sqlEx.Number = 2601 Then
                        ModelState.AddModelError("EM_DuplicateCategoryName", "このカテゴリー名は既に登録されています")
                    End If
                    Return View()
                End Try

                Return RedirectToAction("Index", "Items")
            End If
            Return View(category)
        End Function

        ' GET: Categories/Edit/5
        Function Edit(ByVal id As String) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim category As Category = db.Categories.Find(id)
            If IsNothing(category) Then
                Return HttpNotFound()
            End If
            Return View(category)
        End Function

        ' POST: Categories/Edit/5
        '過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        '詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="CategoryId,CategoryName")> ByVal category As Category) As ActionResult
            If ModelState.IsValid Then
                db.Entry(category).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(category)
        End Function

        ' GET: Categories/Delete/5
        Function Delete(ByVal id As String) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim category As Category = db.Categories.Find(id)
            If IsNothing(category) Then
                Return HttpNotFound()
            End If
            Return View(category)
        End Function

        ' POST: Categories/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As String) As ActionResult
            Dim category As Category = db.Categories.Find(id)
            db.Categories.Remove(category)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace
