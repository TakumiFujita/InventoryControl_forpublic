Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Enum Months
        <Display(Name:="1月")>
        January = 1
        <Display(Name:="2月")>
        February = 2
        <Display(Name:="3月")>
        March = 3
        <Display(Name:="4月")>
        April = 4
        <Display(Name:="5月")>
        May = 5
        <Display(Name:="6月")>
        June = 6
        <Display(Name:="7月")>
        July = 7
        <Display(Name:="8月")>
        August = 8
        <Display(Name:="9月")>
        September = 9
        <Display(Name:="10月")>
        October = 10
        <Display(Name:="11月")>
        November = 11
        <Display(Name:="12月")>
        December = 12
    End Enum
    Public Class EnumDropDownList
        <DisplayName("月")>
        Public Property Month As Months
    End Class
End Namespace
