namespace Acme.ReturnOh.Dtos;

/// <summary>
/// Swagger配置
/// </summary>
/// <param name="Title">Swagger 文档的标题</param>
/// <param name="Version">Swagger 文档的版本，默认为 "v1"</param>
public record SwaggerDocDto(
    string Title,
    string Version);
