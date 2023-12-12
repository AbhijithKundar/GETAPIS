namespace AngularAuthYtAPI.Helpers
{
    public static class EmailBodyUserInfo
    {
        public static string EmailStringBodyUserInfo(string userName, string password)
        {
            return $@"<html>
<head>
</head>
<body style=""margin:0;padding:0;font-family: Arial, Helvetica, sans-serif;"">
<div style=""height: auto; background: linear-gradient(to top, #c9c9ff 50%, #6e6ef6 90%) no-repeat; width:400px; padding:30px"">
<div>
<div>
<h1>User Details</h1>
<hr>
<p>You are recieving this email because you are registered for GET account.</p>
<p>User Name :{userName}</p>
<p>Password : {password}</p>

<br>

<p>Kind Regards,<br><br>
Team GET
</p>
</div>
</div>
</div>
</body>
</html>";
        }
    }
}
