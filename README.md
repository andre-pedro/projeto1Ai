# 1º Projeto de A.I
## Autoria
Projeto em desenvolvimento pelos seguintes alunos:
* [André Pedro](https://github.com/andre-pedro) - Nº a21701115
* [Diogo Maia](https://github.com/IssaMaia) - Nº a21901308
* [Tiago Alves](https://github.com/Synpse) - Nº a21701031
## Distribuição de tarefas
Descriçao da distribuição de tarefas 

## Introdução
Este projeto está a ser desenvolvido no âmbito da disciplina de Inteligencia Artificial, onde nos foi proposto o desenvolvimento de uma simulação de um concerto, onde os agentes presentes no recinto tivessem necessidades como fome, cansaço e divertimento. Foi-nos pedido também que os vários agentes tivessem uma reação a acontecimentos catastróficos (explosões) que ocorrem durante o evento. O projeto encontra-se no seguinte [repositório] de git, bem como o respetivo [enunciado].

//Pequena descrição sobre o problema e a forma como o resolveram. Deve oferecer ao leitor informação suficiente para entender e contextualizar o projeto,
bem como quais foram os objetivos e resultados alcançados.

### Pesquisa
 Deve também ser apresentada nesta secção a pesquisa efetuada sobre simulação baseada em agentes aplicada ao pânico em multidões.

## Metodologia
A simulação foi implementada utilizando a Game Engine Unity, num projeto 3D.

### Agentes
Correndo a sistemas de inteligência artificial nos agentes cinematicos presentes, estes navegam no festival numa _navmesh_. A quantidade de agentes presentes é definida através de um input que pode ser alterado na interface visual ( de X até Y ).

Mediante as condições, estes agentes respondem com os seguintes comportamentos:

* "Seek" - 
* "Idle" - 
* "Flee" - Quando ocorre uma explosão, os agentes num determinado raio 

#### Necessidades

Os agentes possuem duas necessidades: Fome (Hunger) e Cansaço (Tired). Estes parâmetros são randomizados no inicio da simulação. Quando a fome de um agente é igual ou inferior a 25, este abandona o local onde se encontra e vai para a área de restauração. Se o seu cansaço estiver igual ou inferior a 50, este vai procurar uma zona de descanso para recuperar. A permanência de um agente numa área aumenta exponencialmente o valor correspondente. Quando um agente tem ambas as necessidades acima dos valores referidos, avaliam a quantidade de pessoas em cada palco e dirigem-se para o com menos agentes (caso o número de agentes nos dois palcos seja o mesmo, é feito um _random_ para ser tomada a decisão).

#### Reações

Perante algumas situações, os agentes podem reagir de maneiras diferentes do normal. Ao 
Panic
Stunned

### Zonas / Áreas

No recinto do festival existem 3 zonas:

* **Palcos** - 

* **Área de Restauração** - 

* **Zonas de Descanso** - 


### Explosões

O jogador pode escolher com o rato o local onde quer causar uma explosão. Ao clicar com o rato, é criada uma explosão com 3 raios:

* **1º Raio** - Todos os agentes morrem;

* **2º Raio** - Todos os agentes ficam atordoados (_stunned_) (ver secção Reações), ficando  parados num intervalo de tempo entre 2 a 10 segundos. Passado esse tempo, voltam a andar, porém com velocidade de -50%.

* **3º Raio** - Todos os agentes são assustados (_scared_), fugindo na direção oposta da explosão. Se um agente assustado entrar em contacto com outros agentes, estes ficam assustados também.

![anexoRaios](relatorioAnexos/raios.svg)
(Anexo - Raios de Explosão)

*DESCRIÇÃO DE TODAS AS TECNCAS DE IA USADAS(INCLUINDO FIGURAS)
VALORES PARAMETRIZÁVEIS(NUMERO DE AGENTES, VELOCIDADES) E DESCRIÇÃO DA IMPLEMENTAÇÃO(INCLUINDO DIAGRAMA UML SIMPLES OU FLUXOGRAMAS)
ESTA SECÇÃO DEVE TER DETALHE SUFECIENETE PARA QUE OUTRA PESSOA CONSIGA REPLICAR O COMPORTAMENTO DA VOSSA SIMULAÇÃO SEM OLHAR PARA O CODIGO.*
## Resultados e discução
*APRESENTAÇÃO DE RESULTADOS.*
## Conclusão
## Agradecimentos
* Prof. Nuno Fachada
## Referencias
*Adicionar referências*


[repositório]:https://github.com/andre-pedro/projeto1Ai
[enunciado]:https://github.com/VideojogosLusofona/lp2_2019_p1