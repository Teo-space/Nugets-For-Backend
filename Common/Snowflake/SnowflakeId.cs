namespace System;

public record struct Snowflake(long Value)
{
    public static Snowflake New() => new Snowflake(SnowflakeGenerator.New());


    public static implicit operator long(Snowflake snowflakeId) => snowflakeId.Value;
    public static implicit operator string(Snowflake snowflakeId) => snowflakeId.Value.ToString();

}
