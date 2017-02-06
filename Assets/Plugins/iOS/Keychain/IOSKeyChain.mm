#import "SSKeychain.h"

NSString* CreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

char* MakeStringCopy (const char* string)
{
	if (string == NULL) return NULL;
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

extern "C" {

	const char* _Get(const char* service, const char* account)
	{
        return MakeStringCopy([[SSKeychain passwordForService:CreateNSString(service) account:CreateNSString(account)] UTF8String]);
	}
    
    bool _Set(const char* password, const char* service, const char* account)
    {
        return [SSKeychain setPassword:CreateNSString(password) forService:CreateNSString(service) account:CreateNSString(account)];
    }
    
    bool _Delete(const char* service, const char* account)
    {
        return [SSKeychain deletePasswordForService:CreateNSString(service) account:CreateNSString(account)];
    }
}

