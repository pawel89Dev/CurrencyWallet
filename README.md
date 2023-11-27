1. Najpierw należy utworzyć poniższą bazę (można uruchomić w dockerze bazę przy pomocy polecenia docker-compose up w głównym folderze) 

CREATE TABLE "Rates" (
	"Id" SERIAL NOT NULL,
	"Currency" TEXT NOT NULL,
	"Code" TEXT NOT NULL,
	"Rate" NUMERIC(20,10) NOT NULL,
	PRIMARY KEY ("Id")
);

CREATE TABLE "WalletFunds" (
	"RateId" INTEGER NOT NULL,
	"WalletId" INTEGER NOT NULL,
	"Amount" NUMERIC(20,10) NOT NULL
);

CREATE TABLE "Wallets" (
	"Id" SERIAL NOT NULL,
	"Name" TEXT NOT NULL
);

2. Ustawić odpowiednio ConnectionStrings. 
3. Po uruchomieniu aplikacji w przeglądarce otworzy się aplikacj Swagger przy pomocy której można testować api.
4. Przykady użycia zamieszczono w wyeksportowanej kolekcji z programu Postman: CurrencyWallet.postman_collection