# Fundamentos do [Entity Framework](https://docs.microsoft.com/pt-br/ef/)

> Obs.: Com o .NET 6 instalado, foi criado um projeto com a versão 5.

##### Criação do projeto

`dotnet new console -o Blog -f net5.0`

##### Instalação dos pacotes do EF e SQL Server

`dotnet add package Microsoft.EntityFrameworkCore --version 5.0.13`

`dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.13`

## Data Contexts

Contexts são representação das tabelas e seus campos. Para que seja criado um novo Context a classe deverá herdar de `DbContext` do _namespace_ `Microsoft.EntityFrameworkCore`.

As propriedades desta classe serão a representação dos campos da tabela, que são do tipo genérico `DbSet<T>` onde _T_ é a model a qual esta tabela é representada.

Por fim, para finalizar a configuração de um Context é necessário sobreescrever o método _OnConfiguring_ como no exemplo abaixo, utilizado para banco **Sql Server**.

```cs
protected override void OnConfiguring(DbContextOptionsBuilder options)
	=> options.UseSqlServer(CONNECTION_STRING);
```

Perceba que, como o método possui apenas uma única instrução, é utilizado o _Expression Body_ com a sintaxe **=>**.

#### Persistindo Dados

Após configurado o _Data Context_ temos acesso ao _ORM_ do _Entity Framework_, como é aberta uma conexão com o banco e por sua vez esta deve ser fechada, é boa prática a utilização do `using`.

```cs
using (var context = new BlogDataContext())
{
	var tag = new Tag { Name = "ASP.NET", Slug = "aspnet" };

	// Context representa um banco em memória, para que seja persistido os dados
	// é necessário chamar o método SaveChanges().
	context.Tags.Add(tag);
	context.SaveChanges();
}
```

## CRUD

##### Create

```cs
var tag = new Tag { Name = ".NET", Slug = "dotnet" };
context.Tags.Add(tag);
context.SaveChanges();
```

##### Read

```cs
// Ao utilizar AsNoTracking, o EF não trará os metadados que auxiliam nas operações como update delete
// melhorando assim a performance apenas para consultas.
var tags = context
	.Tags
	.AsNoTracking()
	.ToList(); // A query só é executada no ToList()
tags.ForEach(tag => Console.WriteLine(tag.Name));

var tag = context
	.Tags
	.AsNoTracking()
	.FirstOrDefault(x => x.Id == 1); // Busca um registro onde ID = 1
Console.WriteLine(tag?.Name);
```

##### Update

```cs
// Busca do registro a ser atualizado
var tag = context.Tags.FirstOrDefault(x => x.Id == 2);
tag.Name = "Entity Framework"; // Atualização apenas do campo name
context.Tags.Update(tag);
context.SaveChanges();
```

##### Delete

```
// Busca do registro a ser deletado
var tag = context.Tags.FirstOrDefault(x => x.Id == 2);
context.Tags.Delete(tag);
context.SaveChanges();
```
