create database GestioneSpese

create table Categoria(
	Id int identity (1,1) primary key,
	Categoria varchar(100) not null
	)


create table Spesa (
	Id int identity(1,1) primary key,
	[Data] date not null,
	CategoriaId int not null foreign key references Categoria(Id),
	Descrizione varchar(500) not null,
	Utente varchar(100) not null,
	Importo decimal(5,2) not null,
	Approvato bit not null
	)

	insert into Categoria values ('Pasto')			--1
	insert into Categoria values ('Pernottamento')  --2
	insert into Categoria values ('Trasporto')		--3
	insert into Categoria values ('Altro')			--4

	insert into Spesa values ('2021-12-17', 1, 'Pranzo durante trasferta', 'Dipendente349', 21.70, 1)
	insert into Spesa values ('2021-12-09', 2, 'Pernottamento Hotel SuperLuxe 5 stelle, 1 notte', 'Dipendente170', 600.0, 0)
	insert into Spesa values ('2021-12-16', 2, 'Pernottamento Hotel Roma 3 stelle, 2 notti', 'Dipendente349', 160.00, 1)
	insert into Spesa values ('2021-12-17', 3, 'Corsa Taxi', 'Dipendente349', 15.22, 1)
	insert into Spesa values ('2021-12-09', 1, 'Pacchetto Spa Treatment', 'Dipendente3490', 139.99, 0)