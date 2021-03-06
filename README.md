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

## Mapeamento

Entende-se por mapeamento como "dê/para", de relacional para objeto.

Uma classe (Entidade) representará a uma tabela do banco de dados, onde as propriedades são as colunas.

Com mapeamento é possível gerar o banco automaticamente.

#### Tipos de Mapeamento com EF Core

##### Fluent Mapping

São feitos em classe externa, logo não criam dependências na classe/projeto principal e não "poluem" a classe entidade.

##### Data Annotations

São feitos diretamente nas classes, são mais simples e diretos. Dependem do System.ComponentModel.DataAnnotations e por vezes do Microsoft.EntityFrameworkCore.

São utilizados para gerar metadados sobre as classes, são utilizados com o uso do [].

**Exemplo de uma listagem de posts com relacionamento (Post pertence a um Autor)**

```cs
// Utilização de using para dispose automático da classe.
using var context = new BlogDataContext();

var posts = context
    .Posts
    .AsNoTracking()
    .Include(x => x.Author) // Include serve como Join para carregar relacionamentos.
    .Include(x => x.Category) // Traz a categoria relacionada ao post.
    .OrderByDescending(x => x.LastUpdateDate)
    .ToList();
```

**Exemplo de atualização de um registro a partir de seu relacionamento**

```cs
using var context = new BlogDataContext();

var post = context
    .Posts
    .Include(x => x.Author) // Traz o autor relacionado ao post.
    .Include(x => x.Category) // Traz a categoria relacionada ao post.
    .OrderByDescending(x => x.LastUpdateDate) // Ordena pelo mais atual.
    .FirstOrDefault(); // Pega o primeiro registro do select.

post.Author.Name = "Foo Bar"; // Atualizará o nome do autor a qual o post pertence
context.Posts.Update(post);
context.SaveChanges();
```

#### Migrations

Dado um mapeammento, seja este feito através de _fluent mapping_ ou _data annotation_, é possível gerar um banco de dados a partir deste mapeamento.

##### Criando migrations

`dotnet ef migrations add NomeDaMigration`

Ao executar este comando pela primeira vez serão gerados 3 arquivos, que serão estes responsáveis pela geração do banco de dados baseado nas entidades mapeadas.

O arquivo snapshot é um versionamento das migrations.

Também é gerada uma tabela adicional para controle de migrations aplicadas ao banco.

Também é possível gerar o script executado pelo EF com o comando `dotnet ef migrations script -o ./script.sql`

#### Performance

##### AsNoTracking

Utilizado para apenas leitura, como não traz junto os metadados de uma entidade, ele otimiza a leitura descartando informações adicionais que não são necessárias para leitura.

Não deve ser utilizado ao fazer `INSERTS`, `UPDATES` ou `DELETES`.

**Exemplo**

```cs
using var context = new BlogDataContext();
// Se necessário fazer um update ou delete, não utiliza-se o AsNoTracking.
var post = context.Posts.FirstOrDefault(x => x.Id == 3);

// Como serão apenas exibidos os dados, não há necessidade dos metadados
// para melhora de performance utiliza-se o AsNoTracking().
var posts = context.Posts.AsNoTracking().ToList();
```

##### Async/Await

Async e Await é a possibilidade de trabalhar de maneira assíncrona com execução de tarefas em paralelo, no Entity Framework métodos que são assíncronos possuem o sufixo Async e retornam uma **Task** (System.Threading.Tasks).

Métodos assíncronos não esperam por uma execução, a menos que seja utilizado o Await.

**Exemplo**

```cs
static async Task Main(string[] args)
{
    using var context = new BlogDataContext();

    // Caso não seja empregado o uso do await, o programa não irá aguardar
    // a instrução, sendo assim, a string "Teste" será exibida primeira no console.
    var post = await context.Posts.ToListAsync();
    var tags = await context.Tags.ToListAsyn();

    Console.WriteLine("Teste");
}
```

##### Eager Loading/Lazy Loading

Lazy loading pode ser entendido como "carregamento preguiçoso" pois os dados só serão carregados quando chamados.

Por padrão o EF trabalha com Eager Loading (carregamento tardio), pois é necessário explicitar quando os dados devem ser carregados, para incluir relacionamentos deve-se utilizar o Include(), assim o EF faz uma query mais otimizada.

```cs
using var context = new BlogDataContext();

// As tags serão carregadas por Eager Loading.
var post = context.Posts.Include(x => x.Tags);

```

##### Paginação

Em grandes aplicações não é indicado trazer todo conteúdo de um banco de dados de uma vez, para isso o EF nos oferece a opção de paginar nossa busca com o _Skip_ e _Take_ conforme o exemplo abaixo.

```cs
public List<Post> GetPosts(BlogDataContext context, int skip = 0, int take = 25)
{
    // Traz o resultado paginado de 25 em 25
    var posts = context
        .Posts
        .AsNoTracking()
        .Skip(skip)
        .Take(take)
        .ToList();

    return posts;
}
```
