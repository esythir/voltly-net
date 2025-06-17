/* ============================================================
   TEACHER SEED  –  full dataset for professor / reviewer
   ------------------------------------------------------------
   Creates:
     • User  : user@voltly.dev  /  Voltly@123
     • One equipment + one sensor
     • Readings for the last 24 hours (30-min interval)
     • Yesterday’s daily report
     • Current monthly limit
     • One consumption alert (today)
     • One automatic shutdown action
   Can be executed multiple times – previous seed is removed.
   ============================================================*/

DELETE FROM TB_USERS
WHERE  "Email" = 'user@voltly.dev';

DECLARE
v_user_id   TB_USERS."Id"%TYPE;
    v_equipment TB_EQUIPMENTS."Id"%TYPE;
    v_sensor    TB_SENSORS."Id"%TYPE;
BEGIN
    ------------------------------------------------------------
    -- 1) User
    ------------------------------------------------------------
INSERT INTO TB_USERS
("Name", "Email", "Password", "BirthDate",
 "Role", "IsActive", "CreatedAt", "UpdatedAt")
VALUES ('Professor User',
        'user@voltly.dev',
        '$2a$11$1Na7QX2kqfNTvfhoKqeEd.fXt.tbCcaK0zUMzgygUJmP6FUU/N1sG',
        DATE '1990-01-01',
        'USER',
        1,
        SYSTIMESTAMP, SYSTIMESTAMP)
    RETURNING "Id" INTO v_user_id;

------------------------------------------------------------
-- 2) Equipment
------------------------------------------------------------
INSERT INTO TB_EQUIPMENTS
("OwnerId", "Name", "Description",
 "DailyLimitKwh", "Active")
VALUES (v_user_id,
        'AC – Room 101',
        'Primary demo equipment',
        8, 1)
    RETURNING "Id" INTO v_equipment;

------------------------------------------------------------
-- 3) Sensor
------------------------------------------------------------
INSERT INTO TB_SENSORS
("SerialNumber", "Type", "EquipmentId")
VALUES ('SN-DEMO-0001', 'SMART_PLUG', v_equipment)
    RETURNING "Id" INTO v_sensor;

------------------------------------------------------------
-- 4) 24 h of energy readings (30 min)
------------------------------------------------------------
FOR i IN 0 .. 47 LOOP
        INSERT INTO TB_ENERGY_READINGS
              ("SensorId", "PowerKw", "OccupancyPct", "TakenAt")
        VALUES (v_sensor,
                0.20 + DBMS_RANDOM.VALUE(0, 0.05),
                DBMS_RANDOM.VALUE(0, 5),
                SYSTIMESTAMP - (i * 30 / 1440));
END LOOP;

    ------------------------------------------------------------
    -- 5) Yesterday’s daily report
    ------------------------------------------------------------
INSERT INTO TB_DAILY_REPORTS
("EquipmentId", "ReportDate", "ConsumptionKwh",
 "Co2EmissionKg", "EfficiencyRating", "CreatedAt")
VALUES (v_equipment,
        TRUNC(SYSDATE - 1),
        3.4,
        3.4 * 0.0006,
        0,                   -- EfficiencyRating.Good
        SYSTIMESTAMP);

------------------------------------------------------------
-- 6) Monthly limit (current month)
------------------------------------------------------------
INSERT INTO TB_CONSUMPTION_LIMITS
("EquipmentId", "LimitKwh", "ComputedAt")
VALUES (v_equipment,
        4.0,
        TRUNC(ADD_MONTHS(SYSDATE, 0), 'MM'));

------------------------------------------------------------
-- 7) Consumption alert (today)
------------------------------------------------------------
INSERT INTO TB_ALERTS
("EquipmentId", "AlertDate", "ConsumptionKwh",
 "LimitKwh", "ExceededByKwh", "Message", "CreatedAt")
VALUES (v_equipment,
        TRUNC(SYSDATE),
        4.5, 4.0, 0.5,
        'Consumption exceeded by 0.5 kWh',
        SYSTIMESTAMP);

------------------------------------------------------------
-- 8) Automatic action (simulated shutdown)
------------------------------------------------------------
INSERT INTO TB_AUTOMATIC_ACTIONS
("EquipmentId", "Type", "Details", "ExecutedAt")
VALUES (v_equipment,
        'SHUTDOWN',
        'Shutdown triggered by demo seed',
        SYSTIMESTAMP);
END;
/
