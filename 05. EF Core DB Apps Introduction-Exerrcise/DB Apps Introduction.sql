use MinionsDB

select * from Towns
select * from Countries
select * from Minions

select * from Villains

select * from MinionsVillains

select * from EvilnessFactors
order by Id

SELECT v.Name, COUNT(mv.MinionId) [Minions Count] 
  FROM Villains v
  JOIN MinionsVillains mv ON mv.VillainId = v.Id
 GROUP BY v.Name
HAVING COUNT(mv.MinionId)> 3
 ORDER BY [Minions Count] DESC

SELECT v.Name, COUNT(mv.MinionId) [Minions Count]  
FROM Villains v 
JOIN MinionsVillains mv ON mv.VillainId = v.Id 
GROUP BY v.Name 

SELECT Name FROM Villains
 WHERE Id = 1

SELECT v.Name [Villain], m.Name [Minion], m.Age
  FROM Minions m
  JOIN MinionsVillains mv ON mv.MinionId = m.Id
  JOIN Villains v ON v.Id = mv.VillainId
 WHERE mv.VillainId = 1
 GROUP BY v.Name, m.Name, m.Age

---=== 04. Add Minion ===---

select * from Towns

insert into Towns (Name) values
('Plovdiv')