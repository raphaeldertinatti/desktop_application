# Desktop Application with C#

Development of a desktop application using C# (CSharp) and Windows Forms. 

![badge1](https://img.shields.io/badge/Language-C%23-brightgreen)
![badge1](https://img.shields.io/badge/UI%20Framework-WinForms-blue)
![badge3](https://img.shields.io/badge/status-development-red)

## Objective
This repository will contain multiple scripts in C#/Winforms that are usefull for develop desktop applications in general. The application is not for a specific kind of company or operation, but is about a set of scripts to serve different purposes such as a login system, registrations, CRUD, data import, among others.

## Login Screen
<div style="text-align:center;">
<img src="https://github.com/raphaeldertinatti/desktop_application/blob/main/Images/Login_Screen.png" alt="Image">
</div>

First, in `program.cs` the application starts by running the `LoginScreen` Form, and **IF** the variable *LoginSucess* become **"true"** the application runs the Main Form MDI `(Frm_Main)`.

```
static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginScreen Login = new LoginScreen();
            Application.Run(Login);

            if (Login.LoginSucess == true)
            {               
                Application.Run(new Frm_Main());
            } 
```
