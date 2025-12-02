### Shared

#### Example using LoadRecords

```csharp

record Data(BigInteger Num1, BigInteger Num2);

var records = File.ReadAllText("Data2.txt").LoadRecords(
        fields => new Data(BigInteger.Parse(fields[0]), BigInteger.Parse(fields[1]))
    );
```

