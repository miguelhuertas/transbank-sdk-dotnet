[![Build Status](https://travis-ci.org/TransbankDevelopers/transbank-sdk-dotnet.svg?branch=master)](https://travis-ci.org/TransbankDevelopers/transbank-sdk-dotnet)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=dotnetsdk&metric=alert_status)](https://sonarcloud.io/dashboard?id=dotnetsdk)
[![NuGet version](https://badge.fury.io/nu/TransbankSDK.svg)](https://badge.fury.io/nu/TransbankSDK)
# Transbank .Net SDK

SDK Oficial de Transbank

## Requisitos:
 - .Net Standard 1.3+
 - .Net Core 1.0+
 - .Net Framework 4.6+

## Dependencias
Al realizar la instalación con NuGet las dependencias
debieran instalarse automáticamente.

- [Newtonsoft 11.0.2](https://www.newtonsoft.com/json)

## Instalación

### Instalar con NuGet

Desde una línea de comandos:

```bash
nuget install TransbankSDK
```

Desde Package Manager:

```bash
PM> Install-Package TransbankSDK
```

Con .Net CLI:

```bash
dotnet add package TransbankSDK
```

Desde Visual Studio:

1. Abrir el explorador de soluciones.
2. Clic derecho en un proyecto dentro de tu solución.
3. Clic en Administrar paquetes NuGet.
4. Clic en la pestaña Examinar y busque `TransbankSDK`
5. Clic en el paquete `TransbankSDK`, seleccione la versión que desea utilizar y finalmente selecciones instalar.

## Primeros pasos

### Onepay

#### Configuración del ApiKey y SharedSecret

Existen 2 formas de configurar esta información, la cual es única para cada comercio.

##### 1. En la inicialización de tu proyecto. (Solo una vez, al iniciar)

Primero es necesario importar el espacio de nombres:

```csharp
using Transbank.Onepay;
```

La clase `Onepay` contiene la configuración básica de tu comercio.

```csharp
Onepay.ApiKey = "[your api key here]";
Onepay.SharedSecret = "[your shared secret here]";
Onepay.CallbackUrl = "http://www.somecallback.com/example";
```

##### 2. Pasando el ApiKey y SharedSecret a cada petición

Utilizando un objeto `Transbank.Onepay.Model.Options`

```csharp
    TransactionCreateResponse response = Transaction.Create(cart, new Options()
        {
            ApiKey = "[your api key here]",
            SharedSecret = "[your shared secret here]"
        });
```

#### Ambientes TEST y LIVE

Por defecto el tipo de Integración del SDK es siempre: `TEST`.
La clase `OnepayIntegrationType` dentro del espacio de nombres `Transbank.Onepay.Enums` contiene la información de los distintos ambientes disponibles.

Puedes configurar el SDK para utilizar los servicios del ambiente de `LIVE` (Producción) de la suiguiente forma:
```csharp
using Transbank.Onepay;
...
Onepay.IntegrationType = Transbank.Onepay.Enums.OnepayIntegrationType.LIVE;
```

#### Crear una nueva transacción

Para iniciar un proceso de pago mediante la aplicación móvil de Onepay, primero es necesario crear la transacción en Transbank.
Para esto se debe crear en primera instancia un objeto `Transbank.Onepay.Model.ShoppingCart` el cual se debe llenar con ítems
`Transbank.Onepay.Model.Item`

```csharp
using Transbank.Onepay:
using Transbank.Onepay.Model:

//...

ShoppingCart cart = new ShoppingCart();
cart.Add(new Item(
    description: "Zapatos",
    quantity: 1,
    amount: 10000,
    additionalData: null,
    expire: 10));
```
El monto en el carro de compras, debe ser positivo, en caso contrario se lanzará una excepción del tipo
`Transbank.Onepay.Exceptions.AmountException`

Luego que el carro de compras contiene todos los ítems. Se crea la transacción:

```csharp
using Transbank.Onepay:
using Transbank.Onepay.Model:

// ...

TransactionCreateResponse response = Transaction.Create(cart, channel);
```

El parametro channel puede ser `WEB`, `MOBILE` o `APP` dependiendo si quien esta realizando el pago esta usando un browser en 
versión Desktop, Móvil o esta utilizando alguna aplicación móvil nativa.

En caso que `channel` sea `APP` es obligatorio que este previamente configurado el `appScheme`:

```c#
using Transbank.Onepay:

//...

Onepay.AppScheme = "STRINGAPPSCHEME";
```

El resultado entregado contiene la confirmación de la creación de la transacción, en la forma de un objeto `TransactionCreateResponse`.

```json
"occ": "1807983490979289",
"ott": 64181789,
"signature": "USrtuoyAU3l5qeG3Gm2fnxKRs++jQaf1wc8lwA6EZ2o=",
"externalUniqueNumber": "f506a955-800c-4185-8818-4ef9fca97aae",
"issuedAt": 1532103896,
"qrCodeAsBase64": "QRBASE64STRING"
```

En el caso que no se pueda completar la transacción o el `responseCode` en la respuesta del API sea distinta a `ok`
Se lanzara una excepción `Transbank.Onepay.Exceptions.TransactionCreateResponse`

Posteriormente, se debe presentar al usuario el código QR y el número de OTT para que pueda proceder al pago
mediante la aplicación móvil.

#### Confirmar una transacción

Una vez que el usuario realizó el pago mediante la aplicación, dispones de 30 segundos
para realizar la confirmación de la transacción, de lo contrario, se realizará automáticamente
la reversa de la transacción.

```csharp
TransactionCommitResponse commitResponse = Transaction.Commit(
               createResponse.Occ, createResponse.ExternalUniqueNumber);
```

El resultado entregado contiene la confirmación de la confirmación de la transacción, en la forma de un objeto `TransactionCreateResponse`.

```json
"occ": "1807983490979289",
"authorizationCode": "623245",
"issuedAt": 1532104549,
"signature": "FfY4Ab89rC8rEf0qnpGcd0L/0mcm8SpzcWhJJMbUBK0=",
"amount": 27500,
"transactionDesc": "Venta Normal: Sin cuotas",
"installmentsAmount": 27500,
"installmentsNumber": 1,
"buyOrder": "20180720122456123"
```

#### Anular una transacción

Cuando una transacción fue creada correctamente, se dispone de un plazo de 30 días para realizar la anulación de esta.

```csharp
RefundCreateResponse refundResponse = Refund.Create(commitResponse.Amount,
                commitResponse.Occ, response.ExternalUniqueNumber,
                commitResponse.AuthorizationCode);
```

El resultado entregado contiene la confirmación de la anulación, en la forma de un objeto `RefundCreateResponse`.

```json
"occ": "1807983490979289",
"externalUniqueNumber": "f506a955-800c-4185-8818-4ef9fca97aae",
"reverseCode": "623245",
"issuedAt": 1532104252,
"signature": "52NpZBolTEs+ckNOXwGRexDetY9MOaX1QbFYkjPymf4="
```
