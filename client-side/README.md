```

BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.3448/22H2/2022Update) (VMware)
Intel Xeon Gold 6140 CPU 2.30GHz, 2 CPU, 16 logical and 16 physical cores
.NET SDK 7.0.401
  [Host]     : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2
  Job-IHCZFF : .NET 7.0.11 (7.0.1123.42427), X64 RyuJIT AVX2

IterationCount=3  WarmupCount=1  

```
| Method                   | NumberOfSets | Mean     | Error      | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|------------------------- |------------- |---------:|-----------:|---------:|----------:|----------:|----------:|----------:|
| **MeasureSetIntersection**   | **3**            | **10.65 ms** |   **1.384 ms** | **0.076 ms** |  **703.1250** |  **703.1250** |  **703.1250** |    **3.5 MB** |
| MeasureSetUnion          | 3            | 13.51 ms |  14.321 ms | 0.785 ms |  984.3750 |  984.3750 |  984.3750 |   5.15 MB |
| MeasureSetUnionParallel2 | 3            | 26.94 ms |  18.113 ms | 0.993 ms | 1093.7500 | 1062.5000 | 1000.0000 |  12.71 MB |
| MeasureSetUnionParallel  | 3            | 24.93 ms |  14.811 ms | 0.812 ms |  968.7500 |  937.5000 |  937.5000 |  12.71 MB |
| MeasureSetDifference     | 3            | 11.10 ms |   8.802 ms | 0.482 ms |  703.1250 |  703.1250 |  703.1250 |    3.5 MB |
| **MeasureSetIntersection**   | **5**            | **12.59 ms** |   **4.446 ms** | **0.244 ms** |  **953.1250** |  **953.1250** |  **953.1250** |   **5.82 MB** |
| MeasureSetUnion          | 5            | 17.86 ms |  57.831 ms | 3.170 ms |  968.7500 |  968.7500 |  968.7500 |   7.48 MB |
| MeasureSetUnionParallel2 | 5            | 30.49 ms |  26.436 ms | 1.449 ms | 1062.5000 | 1031.2500 | 1000.0000 |  15.73 MB |
| MeasureSetUnionParallel  | 5            | 30.86 ms |  20.450 ms | 1.121 ms | 1062.5000 | 1031.2500 | 1000.0000 |  15.72 MB |
| MeasureSetDifference     | 5            | 13.44 ms |   2.480 ms | 0.136 ms |  953.1250 |  953.1250 |  953.1250 |   5.82 MB |
| **MeasureSetIntersection**   | **7**            | **15.52 ms** |  **19.428 ms** | **1.065 ms** |  **906.2500** |  **906.2500** |  **906.2500** |   **8.14 MB** |
| MeasureSetUnion          | 7            | 19.54 ms |  11.648 ms | 0.638 ms |  968.7500 |  968.7500 |  968.7500 |   9.81 MB |
| MeasureSetUnionParallel2 | 7            | 33.36 ms |   4.141 ms | 0.227 ms | 1066.6667 | 1000.0000 | 1000.0000 |  18.17 MB |
| MeasureSetUnionParallel  | 7            | 30.84 ms |  28.935 ms | 1.586 ms | 1031.2500 | 1000.0000 |  968.7500 |  18.19 MB |
| MeasureSetDifference     | 7            | 18.40 ms |  20.100 ms | 1.102 ms |  937.5000 |  937.5000 |  937.5000 |   8.14 MB |
| **MeasureSetIntersection**   | **10**           | **17.67 ms** |  **18.935 ms** | **1.038 ms** |  **968.7500** |  **968.7500** |  **968.7500** |  **11.62 MB** |
| MeasureSetUnion          | 10           | 26.84 ms |  15.191 ms | 0.833 ms | 1000.0000 | 1000.0000 | 1000.0000 |  13.28 MB |
| MeasureSetUnionParallel2 | 10           | 37.81 ms |  52.955 ms | 2.903 ms | 1142.8571 | 1071.4286 | 1071.4286 |  21.82 MB |
| MeasureSetUnionParallel  | 10           | 38.92 ms |  53.408 ms | 2.927 ms | 1000.0000 | 1000.0000 | 1000.0000 |  21.83 MB |
| MeasureSetDifference     | 10           | 20.16 ms |  22.788 ms | 1.249 ms | 1000.0000 | 1000.0000 | 1000.0000 |  11.61 MB |
| **MeasureSetIntersection**   | **15**           | **21.47 ms** |  **24.789 ms** | **1.359 ms** | **1000.0000** | **1000.0000** | **1000.0000** |  **17.45 MB** |
| MeasureSetUnion          | 15           | 38.55 ms |  11.899 ms | 0.652 ms | 1000.0000 | 1000.0000 | 1000.0000 |  19.12 MB |
| MeasureSetUnionParallel2 | 15           | 45.42 ms |  63.408 ms | 3.476 ms | 1100.0000 | 1000.0000 | 1000.0000 |   27.8 MB |
| MeasureSetUnionParallel  | 15           | 50.42 ms | 109.082 ms | 5.979 ms | 1000.0000 | 1000.0000 | 1000.0000 |  27.83 MB |
| MeasureSetDifference     | 15           | 27.35 ms |  25.651 ms | 1.406 ms | 1000.0000 | 1000.0000 | 1000.0000 |  17.41 MB |
| **MeasureSetIntersection**   | **20**           | **28.91 ms** |  **21.142 ms** | **1.159 ms** |  **968.7500** |  **968.7500** |  **968.7500** |  **23.37 MB** |
| MeasureSetUnion          | 20           | 46.45 ms |  32.213 ms | 1.766 ms | 1090.9091 | 1090.9091 | 1090.9091 |  25.08 MB |
| MeasureSetUnionParallel2 | 20           | 58.74 ms | 121.967 ms | 6.685 ms | 1000.0000 | 1000.0000 | 1000.0000 |   33.7 MB |
| MeasureSetUnionParallel  | 20           | 54.95 ms | 116.073 ms | 6.362 ms | 1000.0000 |  900.0000 |  900.0000 |  33.77 MB |
| MeasureSetDifference     | 20           | 35.78 ms |  28.691 ms | 1.573 ms | 1000.0000 | 1000.0000 | 1000.0000 |  23.37 MB |
