#include <windows.h>
#include <cstdio>
#include <string>
#include <utility>

#pragma comment(lib, "Advapi32.lib")

using namespace std;

const char16_t HIRAGANA[] = u"あいうえお"\
					u"かきくけこ"\
					u"さしすせそ"\
					u"たちつてと"\
					u"なにぬねの"\
					u"はひふへほ"\
					u"まみむめも"\
					u"やゆよ"\
					u"らりるれろ"\
					u"わ"\
					u"がぎぐげご"\
					u"ざじずぜぞ"\
					u"ばびぶべぼ"\
					u"ぱぴぷぺぽ";
#define HIRAGANA_COUNT 	(sizeof(HIRAGANA)/sizeof(WCHAR)-1)

class CryptHandle final{
	HCRYPTPROV	m_hCryptProv = 0;
	HCRYPTHASH	m_hCryptHash = 0;
	PBYTE		m_pHash = nullptr;
public:
	CryptHandle(){}
	~CryptHandle(){
		if(m_hCryptHash){
			::CryptDestroyHash(m_hCryptHash);
			m_hCryptHash = 0;
		}
		if(m_hCryptProv){
			::CryptReleaseContext(m_hCryptProv, 0);
			m_hCryptProv = 0;
		}
		if(m_pHash != nullptr){
			delete [] m_pHash;
			m_pHash = nullptr;
		}
	}

	HCRYPTPROV* CryptProv() { return &m_hCryptProv; }
	HCRYPTHASH* CryptHash() { return &m_hCryptHash; }
	PBYTE Hash() { return m_pHash; }
	PBYTE AllocHash(DWORD size){
		if(m_pHash != nullptr) delete [] m_pHash;
		m_pHash = new BYTE[size];
		return m_pHash;
	}
};

u16string FukkatsunoJumon(LPBYTE hash, DWORD dwSize){
	u16string jumon(u"");
	if(dwSize != 32){
		jumon = u"【じゅもんがちがいます】";
		return move(jumon);
	}

	uint64_t* pu64 = reinterpret_cast<uint64_t*>(hash);

	BYTE wp[4];
	::ZeroMemory(wp, sizeof(wp));
	for(DWORD i=0; i<dwSize; ++i) {
		if(i%3 == 0 && i > 0) {
			jumon += HIRAGANA[wp[0]];
			jumon += HIRAGANA[wp[1]];
			jumon += HIRAGANA[wp[2]];
			jumon += HIRAGANA[wp[3]];
			::ZeroMemory(wp, sizeof(wp));
		}
		switch(i%3) {
			case 0:
				wp[0] = hash[i] >> 2;
				wp[1] = hash[i] << 6;
				wp[1] >>= 2;
			break;
			case 1:
				wp[1] |= hash[i] >> 4;
				wp[2] = hash[i] << 4;
				wp[2] >>= 2;
			break;
			case 2:
				wp[2] |= hash[i] >> 6;
				wp[3] = hash[i] & 0x3F;
			break;
		}
	}
	switch(dwSize%3) {
		case 0:
			jumon += HIRAGANA[wp[0]];
			jumon += HIRAGANA[wp[1]];
			jumon += HIRAGANA[wp[2]];
			jumon += HIRAGANA[wp[3]];
		break;
		case 1:
			jumon += HIRAGANA[wp[0]];
			jumon += HIRAGANA[wp[1]];
			jumon += jumon[*hash % jumon.length()];
			jumon += jumon[*hash % jumon.length() / 2];
		break;
		case 2:
			jumon += HIRAGANA[wp[0]];
			jumon += HIRAGANA[wp[1]];
			jumon += HIRAGANA[wp[2]];
			jumon += jumon[*hash % jumon.length()];
		break;
	}

	// for(DWORD i=0; i<dwSize; ++i){
	// 	jumon += HIRAGANA[(hash[i] % HIRAGANA_COUNT)];
	// }
	return move(jumon);
}

string FormatJumon(u16string& jumon){
	u16string formatJumon(u"");
	string result("");

	int counter = 0;
	for(u16string::iterator it = jumon.begin(); it != jumon.end(); ++it){
		formatJumon += *it;
		++counter;
		if(counter % 11 == 0){
			formatJumon += u"\n";
			counter = 0;
		}else if(counter % 7 == 0 || (counter % 3 == 0 && counter % 6 != 0 && counter % 9 != 0)){
			formatJumon += u"　";
		}
	}
	DWORD size = ::WideCharToMultiByte(CP_ACP, 0, reinterpret_cast<LPCWSTR>(formatJumon.c_str()), -1, nullptr, 0, nullptr, nullptr);
	char *buf = new char[size];
	if(::WideCharToMultiByte(CP_ACP, 0, reinterpret_cast<LPCWSTR>(formatJumon.c_str()), -1, buf, size, nullptr, nullptr)){

		result = buf;
	}
	delete [] buf;
	return move(result);
}

int main(int argc, char** argv)
{
	if(argc < 2){
		puts("dqhash <file>");
		return 1;
	}

	printf("ひらがな数 : %d\n", HIRAGANA_COUNT);

	FILE *pf;
	if((pf=fopen(argv[1], "rb")) == nullptr){
		fprintf(stderr, "File open error : %s\n", argv[1]);
		return 2;
	}

	CryptHandle ch;
	if(!::CryptAcquireContext(ch.CryptProv(), nullptr, nullptr, PROV_RSA_AES, CRYPT_VERIFYCONTEXT)){
		fprintf(stderr, "CryptAcquireContext Error : 0x%08x\n", ::GetLastError());
		return 2;
	}
	if(!::CryptCreateHash(*ch.CryptProv(), CALG_SHA_256, 0, 0, ch.CryptHash())){
		fprintf(stderr, "CryptCreateHash Error : 0x%08x\n", ::GetLastError());
		return 2;
	}

	BYTE buf[32767];
	size_t readsize;
	unsigned long long filesize = 0LLU;
	while((readsize = fread(buf, 1, sizeof(buf), pf)) > 0){
		if(!::CryptHashData(*ch.CryptHash(), buf, readsize, 0)){
			fprintf(stderr, "CryptHashData Error : 0x%08x\n", ::GetLastError());
			return 2;
		}
		filesize += readsize;
	}
	if(ferror(pf)){
		fprintf(stderr, "File read error\n");
		return 2;
	}

	fclose(pf);

	DWORD dwSize = 32;
	ch.AllocHash(dwSize);

	if(!::CryptGetHashParam(*ch.CryptHash(), HP_HASHVAL, ch.Hash(), &dwSize, 0)){
		fprintf(stderr, "CryptGetHashParam Error : 0x%08x\n", ::GetLastError());
		return 2;
	}

	printf("File : %s\n", argv[1]);
	printf("Size : %lluバイト\n", filesize);
	puts("------------------------------------------------");
	printf("そなたにふっかつのじゅもんをおしえよう。\n\n");
	printf(FormatJumon(FukkatsunoJumon(ch.Hash(), dwSize)).c_str());
	// for(DWORD i=0; i<dwSize; ++i){
	// 	printf("%02x", ch.Hash()[i]);
	// }
	// puts("");

	return 0;
}
