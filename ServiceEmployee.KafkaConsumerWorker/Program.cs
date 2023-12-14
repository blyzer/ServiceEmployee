using ServiceEmployee.KafkaConsumerWorker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceEmployee.KafkaConsumerWorker.Services;
using System;
using System.Threading.Tasks;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
// builder.Configuration((hostContext, services) =>
// {
//     var kafkaBootstrapServers = hostContext.Configuration["Kafka:BootstrapServers"];
//     var kafkaTopicName = hostContext.Configuration["Kafka:TopicName"];
//     var kafkaGroupId = hostContext.Configuration["Kafka:GroupId"];
//
//     services.AddSingleton(
//         new KafkaConsumerService<string, string>(kafkaBootstrapServers, kafkaTopicName, kafkaGroupId));
//     services.AddHostedService<KafkaConsumerHostedService>();
// });

// Add services to the DI container.
var kafkaBootstrapServers = builder.Configuration["Kafka:BootstrapServers"];
var kafkaTopicName = builder.Configuration["Kafka:TopicName"];
var kafkaGroupId = builder.Configuration["Kafka:GroupId"];

builder.Services.AddSingleton(new KafkaConsumerService<string, string>(kafkaBootstrapServers, kafkaTopicName, kafkaGroupId));
builder.Services.AddHostedService<KafkaConsumerHostedService>();

// builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.RunAsync();
