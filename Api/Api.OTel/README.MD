Simple test


https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard/standalone?tabs=bash

docker run --rm -it -d \
    -p 18888:18888 \
    -p 4317:18889 \
    --name aspire-dashboard \
    mcr.microsoft.com/dotnet/aspire-dashboard:9.0
	
https://learn.microsoft.com/ru-ru/dotnet/aspire/fundamentals/dashboard/security-considerations?tabs=bash#secure-telemetry-endpoint
	
docker run --rm -it -d -p 18888:18888 -p 4317:18889 --name aspire-dashboard \
    -e DASHBOARD__OTLP__AUTHMODE='ApiKey' \
    -e DASHBOARD__OTLP__PRIMARYAPIKEY='{MY_APIKEY}' \
    mcr.microsoft.com/dotnet/aspire-dashboard:9.0
	

http://192.168.218.172:18888/login?t=bf34deee15c880b3415757f261592584


