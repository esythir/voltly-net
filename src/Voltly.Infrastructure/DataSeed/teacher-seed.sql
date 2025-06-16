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

-- 0) Remove previous demo seed (cascade deletes children)
DELETE FROM TB_USERS WHERE EMAIL = 'user@voltly.dev';

DECLARE
    v_user_id     TB_USERS.ID%TYPE;
    v_equipment   TB_EQUIPMENTS.ID%TYPE;
    v_sensor      TB_SENSORS.ID%TYPE;
BEGIN
    ------------------------------------------------------------
    -- 1) User
    ------------------------------------------------------------
    INSERT INTO TB_USERS
          (NAME, EMAIL, PASSWORD, BIRTHDATE, ROLE, ISACTIVE,
           CREATEDAT, UPDATEDAT)
    VALUES ('Professor User',
            'user@voltly.dev',
            '$2b$11$0AWGF8lvlB2EKU82Xqp2ueL/OXo8OvaW7HkB03JrSRZJ8sg19xcj6', -- BCrypt of Voltly@123
            DATE '1990-01-01',
            'User',
            1,
            SYSTIMESTAMP, SYSTIMESTAMP)
    RETURNING ID INTO v_user_id;

    ------------------------------------------------------------
    -- 2) Equipment
    ------------------------------------------------------------
    INSERT INTO TB_EQUIPMENTS
          (OWNERID, NAME, DESCRIPTION, DAILYLIMITKWH, ACTIVE)
    VALUES (v_user_id,
            'AC – Room 101',
            'Primary demo equipment',
            8, 1)
    RETURNING ID INTO v_equipment;

    ------------------------------------------------------------
    -- 3) Sensor
    ------------------------------------------------------------
    INSERT INTO TB_SENSORS
          (SERIALNUMBER, TYPE, EQUIPMENTID)
    VALUES ('SN-DEMO-0001', 'SMART_PLUG', v_equipment)
    RETURNING ID INTO v_sensor;

    ------------------------------------------------------------
    -- 4) 24 h of energy readings (30-minute slots)
    ------------------------------------------------------------
    FOR i IN 0 .. 47 LOOP
        INSERT INTO TB_ENERGY_READINGS
              (SENSORID, POWERKW, OCCUPANCYPCT, TAKENAT)
        VALUES (v_sensor,
                0.20 + DBMS_RANDOM.VALUE(0, 0.05),  -- kW
                DBMS_RANDOM.VALUE(0, 5),            -- % occupancy
                SYSTIMESTAMP - (i * 30 / 1440));    -- 30 min each
    END LOOP;

    ------------------------------------------------------------
    -- 5) Yesterday’s daily report
    ------------------------------------------------------------
    INSERT INTO TB_DAILY_REPORTS
          (EQUIPMENTID, REPORTDATE, CONSUMPTIONKWH,
           CO2EMISSIONKG, EFFICIENCYRATING, CREATEDAT)
    VALUES (v_equipment,
            TRUNC(SYSDATE - 1),
            3.4,
            3.4 * 0.0006,
            0,               -- EfficiencyRating.Good
            SYSTIMESTAMP);

    ------------------------------------------------------------
    -- 6) Monthly limit (current month)
    ------------------------------------------------------------
    INSERT INTO TB_CONSUMPTION_LIMITS
          (EQUIPMENTID, LIMITKWH, COMPUTEDAT)
    VALUES (v_equipment,
            4.0,
            TRUNC(ADD_MONTHS(SYSDATE, 0), 'MM'));

    ------------------------------------------------------------
    -- 7) Consumption alert (today)
    ------------------------------------------------------------
    INSERT INTO TB_ALERTS
          (EQUIPMENTID, ALERTDATE, CONSUMPTIONKWH,
           LIMITKWH, EXCEEDEDBYKWH, MESSAGE, CREATEDAT)
    VALUES (v_equipment,
            TO_CHAR(TRUNC(SYSDATE), 'YYYYMMDD'),
            4.5, 4.0, 0.5,
            'Consumption exceeded by 0.5 kWh',
            SYSTIMESTAMP);

    ------------------------------------------------------------
    -- 8) Automatic action (simulated shutdown)
    ------------------------------------------------------------
    INSERT INTO TB_AUTOMATIC_ACTIONS
          (EQUIPMENTID, TYPE, DETAILS, EXECUTEDAT)
    VALUES (v_equipment,
            'SHUTDOWN',
            'Shutdown triggered by demo seed',
            SYSTIMESTAMP);
END;
/
