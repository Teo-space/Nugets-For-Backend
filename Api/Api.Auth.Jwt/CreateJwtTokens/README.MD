```

builder.Services.AddOptions<JWTSettings>()
.Bind(builder.Configuration.GetSection(nameof(JWTSettings)));

```