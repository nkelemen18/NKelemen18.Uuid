# UUID generator and decoder library

Based on new UUID formats RFC Draft: [RFC4122](https://datatracker.ietf.org/doc/draft-ietf-uuidrev-rfc4122bis/)

### Installation

This package is available on NuGet: [NKelemen18.Uuid](https://www.nuget.org/packages/NKelemen18.Uuid)

```shell
nuget install NKelemen18.Uuid
```

### Description

This library currently only support UUIDv7.

- It uses unix timestamp with `millisecond` precision
- Implements **Fixed-Length Dedicated Counter Bits (Method 1)**. (See: RFC4122 draft Section 6.2) It uses a 12 bit
  sequence as `rand_a`. This sequence is set to a pseudo-random number on every time tick.

UUIDv7

```text
 0                   1                   2                   3
 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
|                           unix_ts_ms                          |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
|          unix_ts_ms           |  ver  |       rand_a          |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
|var|                        rand_b                             |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
|                            rand_b                             |
+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
```

### Usage

##### Using static wrapper class

For simple use cases there are two static classes named `Uuid` and `UuidV7`

Using `Uuid` class

```csharp
using NKelemen18.Uuid;

Console.WriteLine(Uuid.NewUuidV7());
Console.WriteLine(Uuid.NewUuidV7());
Console.WriteLine(Uuid.NewUuidV7());
```

Using `UuidV7` class

```csharp
using NKelemen18.Uuid.v7;

Console.WriteLine(UuidV7.NewUuidV7());
Console.WriteLine(UuidV7.NewUuidV7());
Console.WriteLine(UuidV7.NewUuidV7());
```

Example output

```text
018a6154-146d-703c-9346-5d58909a61a4
018a6154-1476-73ab-b3b1-df81d873d9ec
018a6154-1477-7424-83de-5e55b9abc57c
```

##### Using generator

```csharp
using NKelemen18.Uuid.v7;

// You can modify the random sequence boundaries or disable sequene at all
var generatorOptions = new UuidV7GeneratorOptions(
    sequenceStartMinValue: 0,
    sequenceStartMaxValue: 2048,
    useSequence: true
);
var generator = new UuidV7Generator(generatorOptions);
Console.WriteLine(generator.NewUuidV7());
Console.WriteLine(generator.NewUuidV7());

// You can generate UUIDv7 for custom timestamp
var customTimeStamp = new DateTime(2025, 9, 4, 18, 18, 18, DateTimeKind.Utc);
Console.WriteLine(generator.NewUuidV7(customTimeStamp));
Console.WriteLine(generator.NewUuidV7(customTimeStamp));
```

Example output

```text
018a615b-76d1-776e-ac7c-d821dee7ac53
018a615b-76d8-763e-b492-bcccbca30366
019915f3-6a10-7360-b90a-d1a7c960f2f7
019915f3-6a10-7361-9fc4-61d3aea96709
```

##### Decode

```csharp
using NKelemen18.Uuid.v7;

// Using guids generated above
Console.WriteLine(UuidV7Decoder.Decode("019915f3-6a10-7360-b90a-d1a7c960f2f7"));
Console.WriteLine(UuidV7Decoder.Decode(new Guid("019915f3-6a10-7361-9fc4-61d3aea96709")));
```

Output `(utc timestamp, sequence)`

```text
(2025. 09. 04. 18:18:18, 864)
(2025. 09. 04. 18:18:18, 865)
```