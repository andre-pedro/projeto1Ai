# 1º Projeto de A.I

## Autoria
Projeto em desenvolvimento pelos seguintes alunos:
* [André Pedro](https://github.com/andre-pedro) - Nº a21701115
* [Diogo Maia](https://github.com/IssaMaia) - Nº a21901308
* [Tiago Alves](https://github.com/Synpse) - Nº a21701031

## Distribuição de tarefas
//Descrição da distribuição de tarefas 

## Introdução
Este projeto está a ser desenvolvido no âmbito da disciplina de Inteligencia Artificial, onde nos foi proposto o desenvolvimento de uma simulação de um concerto, onde os agentes presentes no recinto tivessem necessidades como fome e cansaço. Foi-nos pedido também que os vários agentes tivessem uma reação a acontecimentos catastróficos (explosões) que ocorrem durante o evento.
 O projeto encontra-se no seguinte [repositório] de git, bem como o respetivo [enunciado].

//Pequena descrição sobre o problema e a forma como o resolveram. Deve oferecer ao leitor informação suficiente para entender e contextualizar o projeto,
bem como quais foram os objetivos e resultados alcançados.

### Pesquisa
 Deve também ser apresentada nesta secção a pesquisa efetuada sobre simulação baseada em agentes aplicada ao pânico em multidões.

## Metodologia
A simulação foi implementada utilizando a Game Engine Unity, num projeto 3D.

### Agentes

#### Pathfinding
Recorrendo a sistemas de inteligência artificial nos agentes, criámo-los com um tipo de movimento cinemático e fazendo com que navegam pelo festival numa _navmesh_. A quantidade de agentes presentes é definida através de um input que pode ser alterado na interface visual ( de X até Y ).

Existem 2 maneiras em que os agentes podem navegar pelo recinto: pelos caminhos pré-definidos ou pelo caminho mais curto. Os caminhos pré-definidos têm um custo menor que o caminho mais curto, por isso, numa situação normal os agentes seguem os caminhos para eles destinados.
No entanto, na presença de uma explosão (ver secção correspondente), o custo do caminho mais curto fica menor que o pré-definido, fazendo com que fujam pelo caminho mais curto e não só pelos destinados.

paths have price. no caminho has expensive price. when explosion, caminho out becomes cheaper

Mediante as condições, estes agentes respondem com os seguintes comportamentos:

* "**Seek**" - Sendo a acção que dá a maior "autonomia" aos agentes, o comportamento _seek_ acontece sempre que cada agente precisa de se deslocar para um local (por exemplo, quando alguém se desloca para um palco ou outra zona).
* "**Idle**" - Considerado o comportamento "neutro", os agentes ficam no estado _idle_ quando não estão a efetuar nenhum dos restantes comportamentos, ocorrendo frequentemente quando os agentes estão a recuperar alguma necessidade.
* "**Flee**" - Quando ocorre uma explosão, os agentes num determinado raio 

#### Necessidades dos Agentes

Os agentes possuem duas necessidades: Fome (Hunger) e Cansaço (Tired). Estes parâmetros são randomizados no inicio da simulação. Quando a fome de um agente é igual ou inferior a 25, este abandona o local onde se encontra e vai para a área de restauração. Se o seu cansaço estiver igual ou inferior a 50, este vai procurar uma zona de descanso para recuperar.
A permanência de um agente numa área aumenta exponencialmente o valor correspondente. Quando um agente tem ambas as necessidades acima dos valores referidos, avaliam a quantidade de pessoas em cada palco e dirigem-se para o com menos agentes (caso o número de agentes nos dois palcos seja o mesmo, é feito um _random_ para ser tomada a decisão).

### Zonas / Áreas

No recinto do festival existem 3 zonas:
* **Palcos** - Zona onde os agentes permanecem durante a maior parte do festival. Quando decidem ir para um dos palcos, é escolhido um ponto aleatório na primeira fila. Os agentes tentam aproximar-se o máximo que conseguirem sem empurrar outros agentes.

* **Área de Restauração** - Local com diversas mesas e cadeiras onde os agentes se deslocam quando a sua fome desce, permanecendo no local até o valor ficar acima de 50. Ao dirigir-se para o local, o agente reserva um lugar o mais afastado possível dos restantes. Quando um agente escolhe um lugar, só ele é que pode lá ficar.

* **Zonas de Descanso** - Local onde os agentes se deslocam quando o seu cansaço desce, permanecendo no local até o valor ficar acima de 50. 



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