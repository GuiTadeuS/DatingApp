### DatingApp
___
##### Technologies Used:
<pre>
*Frontend*:
    - Angular 17
    - Nginx (Load balancer)
*Backend*:
    - ASP.NET Core (using the repository pattern)
*Database*:
    - PostgreSQL
*Observability*:
    - OpenTelemetry (data collection)
    - Grafana (visualization)
    - Tempo (tracing backend)
</pre>
##### Running the Project:

```
docker-compose up -d --build
```

##### Explanation of Technologies:

+ *OpenTelemetry*: Generate telemetry data (metrics, logs, and traces).

+ *Grafana*: Visualizing and analyzing data collected by OpenTelemetry.

+ *Tempo*: Stores and retrieves trace data for analysis.
