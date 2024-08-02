using IdGen;

namespace System;

public static class SnowflakeGenerator
{
    public static long New() => Generator.CreateId();


    public static DateTime Epoch => new DateTime(2024, 8, 2, 17, 0, 0, DateTimeKind.Utc);

    public const byte TimestampBits = 43;
    public const byte GeneratorIdBits = 0;
    public const byte SequenceBits = 20;
    public const int GeneratorId = 0;

    public readonly static IdStructure Structure = new IdStructure(TimestampBits, GeneratorIdBits, SequenceBits);
    public readonly static IdGeneratorOptions GeneratorOptions = new IdGeneratorOptions(Structure, new DefaultTimeSource(Epoch));
    public readonly static IdGenerator Generator = new IdGenerator(GeneratorId, GeneratorOptions);


    public static string Info()
    {
        return $@"
Max. generators       : {Structure.MaxGenerators}
Id's/ms per generator : {Structure.MaxSequenceIds}
Id's/ms total         : {Structure.MaxGenerators * Structure.MaxSequenceIds}
Wraparound interval   : {Structure.WraparoundInterval(Generator.Options.TimeSource)}
Wraparound date       : {Structure.WraparoundDate(Generator.Options.TimeSource.Epoch, Generator.Options.TimeSource).ToString("O")}
";
    }

}
