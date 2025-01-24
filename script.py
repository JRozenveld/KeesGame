from collections import defaultdict

# De vernieuwde data
revised_entries_with_names = [
    (["Kevin"], 1.5),
    (["Robin", "Kevin"], 2),
    (["Robin"], 2),
    (["Robin", "Kevin"], 1.5),
    (["Robin", "Kevin"], 1.5),
    (["Robin", "Kevin"], 1.5),
    (["Robin", "Kevin"], 1),
    (["Robin", "Kevin"], 1.5),
    (["Robin", "Jens", "Kevin"], 2.5),
    (["Robin"], 4.5),
    (["Jens"], 4.5),
    (["Kevin"], 5),
    (["Jens"], 5),
    (["Robin", "Jens", "Kevin"], 4),
    (["Robin", "Jens", "Kevin"], 4.5),
    (["Robin", "Jens", "Kevin"], 4),
    (["Jens", "Kevin"], 3),
    (["Robin", "Jens", "Kevin"], 5),
    (["Robin", "Jens", "Kevin"], 3),
    (["Robin", "Jens", "Kevin"], 2),
    (["Robin", "Jens", "Kevin"], 2),
    (["Robin", "Kevin"], 2),
    (["Robin", "Jens", "Kevin"], 11),
    (["Robin", "Jens", "Kevin"], 13),
    (["Jens"], 5),
    (["Kevin"], 5),
    (["Robin", "Jens", "Kevin"], 5),
    (["Robin", "Jens", "Kevin"], 7.5),
    (["Robin", "Jens", "Kevin"], 5),
    (["Robin", "Jens", "Kevin"], 7),
    (["Robin", "Jens", "Kevin"], 8),
    (["Robin", "Jens"], 4),
    (["Robin", "Jens", "Kevin"], 4),
    (["Jens"], 4),
    (["Robin", "Jens", "Kevin"], 3),
    (["Robin", "Jens"], 4),
    (["Robin", "Jens", "Kevin"], 2),
    (["Robin", "Jens", "Kevin"], 2),
    (["Robin", "Jens", "Kevin"], 2),
    (["Robin", "Jens", "Kevin"], 2),
    (["Robin", "Jens", "Kevin"], 2),
    (["Robin", "Jens", "Kevin"], 2),
    (["Robin", "Jens", "Kevin"], 2),
    (["Robin", "Jens", "Kevin"], 2),
    (["Jens"], 10),
    (["Kevin"], 4),
    (["Robin"], 6),
]


# Uren berekenen met de vernieuwde data
final_hours_worked = defaultdict(float)

for names, hours in revised_entries_with_names:
    # Als Robin, Jens, en Kevin allemaal aanwezig zijn, geef dan de volledige uren aan elk
    if set(names) == {"Robin", "Jens", "Kevin"}:
        for name in names:
            final_hours_worked[name] += hours
    else:
        # Anders de uren gelijk verdelen
        shared_hours = hours / len(names)
        for name in names:
            final_hours_worked[name] += shared_hours

# Resultaten afdrukken
for name, hours in final_hours_worked.items():
    print(f"{name}: {hours:.2f} uur")
