string password = "12345678";

print("Start Hash");
var hashedPassword = PasswordHasher.String.Hash(password);
print("Hashed");

print(hashedPassword.Hash);
print(hashedPassword.Salt);

print("Start Verify");
var varify = PasswordHasher.String.Verify(password, hashedPassword.Hash, hashedPassword.Salt);
print("Verified");

print(varify);


