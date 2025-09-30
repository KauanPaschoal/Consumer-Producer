# Projeto Producer/Consumer com RabbitMQ

## Integrantes

- **Kauan Paschoal**  
  **RA:** 

---

## Estrutura do Projeto

O arquivo `.zip` inclui:

- `/producer` - Aplicação Java Spring Boot (Produtor)
- `/consumer` - Aplicação .NET Core (Consumidor)
- `docker-compose.yml` - Orquestração dos containers Producer, Consumer e RabbitMQ
- `README.md` - Este documento explicativo

---

## Como Enviar uma Mensagem (Teste do Produtor)

### **Endpoint**
- **URL:** `http://localhost:8080/publish`

### **Método HTTP**
- **POST**

### **Headers**
- `Content-Type: application/json`

### **Exemplo de JSON**
```json
{
  "id": "id_123",
  "title": "Titulo da mensagem",
  "body": "Corpo da mensagem"
}
```

### **Exemplo via curl**
```sh
curl -X POST http://localhost:8080/publish \
  -H "Content-Type: application/json" \
  -d '{"id":"id_123","title":"Titulo da mensagem","body":"Corpo da mensagem"}'
```

---

## Passo a Passo para Subir o Ambiente

1. **Clone ou extraia o .zip**
   ```sh
   unzip seu-projeto.zip
   cd seu-projeto
   ```

2. **Suba os serviços com Docker Compose**
   ```sh
   docker-compose up -d
   ```
   Isso iniciará:
   - RabbitMQ (porta 5672 para aplicações, 15672 para painel web)
   - Producer (porta 8080)
   - Consumer (porta 5000)

3. **Verifique se os containers estão rodando**
   ```sh
   docker ps
   ```
   Você deve ver os três serviços em execução.

---

## Como Testar o Sistema

### 1. **Publicar mensagem (Producer)**
- Faça um POST conforme exemplo acima.

### 2. **Verificar mensagem no Consumer**
- Acesse o endpoint GET do consumidor:
  - **URL:** `http://localhost:5000/messages`
  - **Método:** GET

- Exemplo via curl:
  ```sh
  curl http://localhost:5000/messages
  ```

- O retorno esperado é um JSON com as mensagens recebidas da fila RabbitMQ:
  ```json
  [
    {
      "id": "id_123",
      "title": "Titulo da mensagem",
      "body": "Corpo da mensagem"
    }
  ]
  ```

### 3. **(Opcional) Verificar RabbitMQ via painel**
- Acesse: [http://localhost:15672](http://localhost:15672)
- Usuário/Senha: `guest` / `guest`
- Veja as filas e mensagens em tempo real.

---

## Observações Importantes

- **Ambiente:** Docker é obrigatório, não precisa instalar Java/.NET/RabbitMQ manualmente.
- **Fila usada:** `horizon_queue` (deve ser igual em ambas aplicações).
- **Problemas comuns:** Se não funcionar, verifique se o Producer está conectando ao RabbitMQ usando o hostname `rabbitmq` (não `localhost`) no `application.properties`.

---

## Talvez você tenha que rodar novamente o container de .Net para efetuar o teste!
