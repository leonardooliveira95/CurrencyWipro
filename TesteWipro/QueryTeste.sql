create table tb_Status(
	idStatus int not null identity primary key,
	dsStatus varchar(50) not null
);

create table tb_Processo (
	idProcesso int not null identity primary key,
	nroProcesso bigint not null,
	Autor varchar(90) null,
	DtEntrada date null,
	DtEncerramento date null,
	idStatus int null,
	constraint fk_tb_Processo_tb_Status foreign key (idStatus) references tb_status(idStatus)
);

create table tb_Andamento(
	idAndamento int not null identity primary key,
	idProcesso int not null,
	dtAndamento date null,
	dsMovimento varchar(2000) null
	constraint fk_tb_Andamento_tb_Processo foreign key (idProcesso) references tb_Processo(idProcesso)
);

insert into tb_Status (dsStatus) values ('Status 1'), ('Status 2'), ('Status 3');
insert into tb_Processo (nroProcesso, autor, dtentrada, DtEncerramento, idStatus) values (1, 'Autor 1', getdate(), getdate() + 1, 1);
insert into tb_Processo (nroProcesso, autor, dtentrada, DtEncerramento, idStatus) values (2, 'Autor 2', getdate(), getdate() + 1, 2);
insert into tb_Processo (nroProcesso, autor, dtentrada, DtEncerramento, idStatus) values (3, 'Autor 3', getdate(), getdate() + 1, 3);
insert into tb_Processo (nroProcesso, autor, dtentrada, DtEncerramento, idStatus) values (4, 'Autor 1', getdate(), getdate() + 1, 1);
insert into tb_Processo (nroProcesso, autor, dtentrada, DtEncerramento, idStatus) values (5, 'Autor 2', getdate(), getdate() + 1, 2);
insert into tb_Processo (nroProcesso, autor, dtentrada, DtEncerramento, idStatus) values (6, 'Autor 3', getdate(), getdate() + 1, 3);
insert into tb_Processo (nroProcesso, autor, dtentrada, DtEncerramento, idStatus) values (7, 'Autor 1', getdate(), getdate() + 1, 1);
insert into tb_Processo (nroProcesso, autor, dtentrada, DtEncerramento, idStatus) values (8, 'Autor 2', getdate(), getdate() + 1, 2);
insert into tb_Processo (nroProcesso, autor, dtentrada, DtEncerramento, idStatus) values (9, 'Autor 3', getdate(), getdate() + 1, 3);
insert into tb_Processo (nroProcesso, autor, dtentrada, DtEncerramento, idStatus) values (10, 'Autor 3', getdate(), getdate() + 1, 3);

insert into tb_Andamento (idProcesso, dtAndamento, dsMovimento) values (1, getdate(), 'Movimento 1');
insert into tb_Andamento (idProcesso, dtAndamento, dsMovimento) values (2, getdate(), 'Movimento 2');
insert into tb_Andamento (idProcesso, dtAndamento, dsMovimento) values (3, getdate(), 'Movimento 3');
insert into tb_Andamento (idProcesso, dtAndamento, dsMovimento) values (4, getdate(), 'Movimento 4');



--1. Com base no modelo acima, escreva um comando SQL que liste a quantidade de processos por Status com sua descrição.

select count(*) as contagem_processos, tbs.dsStatus
from tb_Processo as tbp
inner join tb_Status as tbs on tbp.idStatus = tbs.idStatus
group by tbs.dsStatus

--2. Com base no modelo acima, construa um comando SQL que liste a maior data de andamento por número de processo, com processos encerrados no ano de 2013.
select max(dtandamento), tbp.nroProcesso
from tb_Andamento as tba
inner join tb_Processo as tbp on tba.idProcesso = tbp.idProcesso
where year(tbp.DtEncerramento) = 2013
group by tbp.nroProcesso

--3. Com base no modelo acima, construa um comando SQL que liste a quantidade de Data de Encerramento agrupada por ela mesma onde a quantidade da contagem seja maior que 5.
select count(dtencerramento)
from tb_Processo as tbp
group by DtEncerramento
having count(dtencerramento) > 5

--4. Possuímos um número de identificação do processo, onde o mesmo contém 12 caracteres com zero à esquerda, 
--contudo nosso modelo e dados ele é apresentado como bigint. Como fazer para apresenta-lo com 12 caracteres 
--considerando os zeros a esquerda?

--SQL Server 2012+
select format(nroProcesso, '000000000000')
from tb_Processo

--All SQL Server 
select right('000000000000' + cast(nroProcesso as varchar(30)), 12)
from tb_Processo
