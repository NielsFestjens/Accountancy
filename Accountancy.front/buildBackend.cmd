cd ..
cd Accountancy.Back
dotnet ef database drop -f
dotnet ef database update
dotnet run