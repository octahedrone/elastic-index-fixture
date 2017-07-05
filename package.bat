msbuild.exe xUnit.ElasticFixture.sln /property:Configuration=Release

nuget pack -OutputDirectory Release -IncludeReferencedProjects -Properties Configuration=Release

pause