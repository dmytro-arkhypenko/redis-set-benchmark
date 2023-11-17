
# Redis Benchmarking Project

## Overview

This repository contains two benchmarking projects for Redis operations using the .NET platform. The benchmarks focus on evaluating the performance of set operations such as intersection, union, and difference on Redis data. The projects utilize BenchmarkDotNet for performance measurement.

## Prerequisites

- .NET 6.0 or later.
- A Redis server instance accessible via an endpoint.
- Basic knowledge of C# and Redis operations.

## Getting Started

1. Clone the repository to your local machine.
2. Ensure you have a running Redis server and note down its endpoint.

## Projects

### 1. `ClientBenchmark`

#### Description

This project benchmarks Redis operations using serialized and compressed data. It uses MessagePack for serialization and LZ4 compression.

#### Key Operations

- Set Intersection
- Set Union (Serial and Parallel)
- Set Difference

### 2. `RedisBenchmark`

#### Description

This project benchmarks Redis operations using native Redis data types and operations.

#### Key Operations

- Set Intersection
- Set Union
- Set Difference

## Usage

To run the benchmarks:

1. Open the terminal in the project directory.
2. Run the command:

   ```sh
   dotnet run --project [ProjectName] [RedisEndpoint]
   ```

   Replace `[ProjectName]` with `ClientBenchmark` or `RedisBenchmark` and `[RedisEndpoint]` with your Redis server endpoint.

## Results
![bm1](https://github.com/dmytro-arkhypenko/redis-set-benchmark/assets/150142594/cca765d1-b574-478b-924a-3b6d1224961c)

## Contributing

Contributions are welcome! Feel free to submit pull requests or create issues for bugs and feature requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

Note: Ensure that your Redis server endpoint is kept secure and not exposed publicly. This benchmark suite is intended for development and testing purposes and should not be used on production servers without proper consideration of the potential performance impact.



