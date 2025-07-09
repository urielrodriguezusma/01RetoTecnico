### Welcome to the TransactionService ###

The idea of this template is create a base application. To install the template follow the next steps.

1. Open a shell and update the path into the ApiTemplate folder.
2. Execute the next command. 

```shell
dotnet new install .
```

You will see the message that the new tamplate has been installed.

```shell
Template Name    Short Name  Language  Tags
---------------  ----------  --------  -----------
MarketApp Api    market-api    [C#]    Web/ASP.NET
```
If the template has been installed before and you need to updated, you 
can execute the command

```shell
dotnet new install . --force
```

If you want to see the list of the available templates you can use the command

```shell
dotnet new list
```

3. Now that the template was install, you can go to any folder. (The name should be the name that we want to the project)

4. Open a new terminal, pointing to the new folder path, and execute the next command: 

```shell
dotnet new market-api
```

5. With these steps you should have a new project with the basic funcionality.

Note: For the documentation of the template symbol this is the reference
[Create template reference](https://github.com/dotnet/templating/wiki/Reference-for-template.json#computed-symbol)



## Nuget packate published in github packages ##

1. Add the nuget package source with this Url
```shell
https://nuget.pkg.github.com/urielrodriguezusma/index.json
```

2 ```shell
dotnet new install marketapp.Templates::1.0.0
```


