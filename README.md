# graphql-asp-core-exemplo
projeto criado para estudo de graphql usando  .net core 2.2

Pacotes usados.

 * GraphQL
 * GraphQL.Server.Transports.AspNetCore
 * GraphQL.Server.Ui.Playground




# Exemplos de Querys e Mutations do projeto.
 
 * query simples  

```javascript
 query {
  post
  {
    id,
    description   
  }
} 
```
 ![alt text](https://github.com/leandro0404/graphql-asp-core-exemplo/blob/master/images/graphql-request.png)

* copie a o trecho abaixo para começar entender  o formato de consultas
 ```javascript
query {
  post
(
  pageSettings:{
    pageIndex:0,
    pageSize:2,
    sortSettings:{
      orderBy:"id",
      direction:DESC 
    }
  }
)
  {
    id
    title,
    description ,
    likes,
    author{
      id,name
    },
    comment (
      pageSettings:{
        pageIndex:0,
        pageSize:15,
        sortSettings:{
          orderBy:"id",
          direction:DESC
        }
      }
    ){
      id,
      text
    }
    author{
     id
    }
  }
  
}
```
![alt text](https://github.com/leandro0404/graphql-asp-core-exemplo/blob/master/images/query_exemplo_passando_filtro.png)

```javascript
mutation Post($post: PostInputType!) {
  createPost(postInput: $post) {
    id
    description
    title
  }
}
```

* query variables
```javascript
{
  "post": {
    "description": "name",
    "title": "teste"
  }
}
````

![alt text](https://github.com/leandro0404/graphql-asp-core-exemplo/blob/master/images/mutation_exemplo_criando_post.png)


* consultas complexas envolvendo filtros com join em itens filhos
```javascript
query {
 post
(
  filter:{
    author:{
      id:2
    }
  },
    pageSettings:{
    pageIndex:0,
    pageSize:15,
    sortSettings:{
    orderBy:"title",
    direction:ASC
    }
  }
)
 {
   id
   title,
   description ,
   likes,
   author{
     id,name
   }
 }
 
}
```
![alt text](https://github.com/leandro0404/graphql-asp-core-exemplo/blob/master/images/query_exemplo_passando_filtro_com_join.png)



