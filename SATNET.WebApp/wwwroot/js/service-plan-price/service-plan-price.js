
$(document).ready(function () {
    if ($('#ServicePlanModel_PlanTypeId').val() === PLAN_TYPE.get('Quota')) {
        $('.plan-validity').show();
    }
    else {
        $('.plan-validity').hide();
    }
});
$('#ServicePlanModel_PlanTypeId').change(function () {
    $('#ServicePlanModel_PlanValidity').val('');
    if ($('#ServicePlanModel_PlanTypeId').val() === PLAN_TYPE.get('Quota')) {
        if ($('#ServicePlanModel_Name').val() !== '') {
            $('#ServicePlanModel_PlanValidity').val($('#ServicePlanModel_Name').val() + ' ' + PLAN_VALIDITY);
        }
        $('.plan-validity').show();
    }
    else {
        $('.plan-validity').hide();
    }

});
$('#ServicePlanModel_Name').change(function () {
    if ($('#ServicePlanModel_PlanTypeId').val() === PLAN_TYPE.get('Quota')) {
        $('#ServicePlanModel_PlanValidity').val('');
        $('#ServicePlanModel_PlanValidity').val($('#ServicePlanModel_Name').val() + ' ' + PLAN_VALIDITY);
    }
});